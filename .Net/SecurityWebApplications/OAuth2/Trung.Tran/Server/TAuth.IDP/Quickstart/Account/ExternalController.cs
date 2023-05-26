using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TAuth.IDP;
using TAuth.IDP.Models;
using TAuth.IDP.Services;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<ExternalController> _logger;
        private readonly IEventService _events;
        private readonly IIdentityService _identityService;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IIdentityService identityService,
            ILogger<ExternalController> logger,
            UserManager<AppUser> userManager)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            //_users = users ?? new TestUserStore(TestUsers.Users);

            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _identityService = identityService;
            _logger = logger;
            _events = events;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Challenge(string scheme, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            if (scheme == AuthConstants.IIS.AuthDisplayName)
            {
                return await ChallengeWindowsAsync(returnUrl);
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = User.Identity.IsAuthenticated ? Url.Action(nameof(CallbackLinking)) : Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };

            return Challenge(props, scheme);

        }

        /// <summary>
        /// Post processing of external account linking
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CallbackLinking()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                var currentUserSub = User.FindFirstValue(JwtClaimTypes.Subject);

                if (currentUserSub == null) throw new InvalidOperationException();

                user = await _userManager.FindByIdAsync(currentUserSub);

                var identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

                if (!identityResult.Succeeded)
                {
                    SetIdentityResultErrors(identityResult);
                    return View("Message", new MessageViewModel());
                }

                return View("Message", new MessageViewModel() { Message = "Linked account successfully" });
            }

            ModelState.AddModelError("", "External identity already has an account in system");
            return View("Message", new MessageViewModel());
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                var userClaims = TransformExternalClaims(provider, claims);
                var fillInfoVm = BuildFillInformationViewModel(userClaims, returnUrl);
                return View("FillInformation", fillInfoVm);
            }

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (!user.Active)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(user.UserName, "email not confirmed", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.EmailNotConfirmedErrorMessage);

                return View("Message", new MessageViewModel());
            }

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            var isuser = new IdentityServerUser(user.Id)
            {
                DisplayName = user.Email,
                IdentityProvider = provider,
                AdditionalClaims = additionalLocalClaims
            };

            await HttpContext.SignInAsync(isuser, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, user.UserName, true, context?.Client.ClientId));

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnUrl);
                }
            }

            return Redirect(returnUrl);
        }

        private async Task<IActionResult> ChallengeWindowsAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AuthConstants.IIS.AuthDisplayName);
            if (result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the
                // user back to the external callback, in essence, treating windows
                // auth the same as any other external authentication mechanism
                var props = new AuthenticationProperties()
                {
                    RedirectUri = User.Identity.IsAuthenticated ? Url.Action(nameof(CallbackLinking)) : Url.Action(nameof(Callback)),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", AuthConstants.IIS.AuthDisplayName },
                    }
                };

                var id = new ClaimsIdentity(AuthConstants.IIS.AuthDisplayName);

                // the sid is a good sub value
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.FindFirst(ClaimTypes.PrimarySid).Value));

                // the account name is the closest we have to a display name
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                // add the groups as claims -- be careful if the number of groups is too large
                var wi = wp.Identity as WindowsIdentity;

                // translate group SIDs to display names
                var groups = wi.Groups.Translate(typeof(NTAccount));
                var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                id.AddClaims(roles);

                await HttpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }
            else
            {
                // trigger windows auth
                // since windows auth don't support the redirect uri,
                // this URL is re-triggered when we call challenge
                return Challenge(AuthConstants.IIS.AuthDisplayName);
            }
        }

        private async Task<(AppUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        // Not used because we will need additional claims
        private async Task<(AppUser NewUser, IdentityResult IdentityErrorResult)>
            AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            var userClaims = TransformExternalClaims(provider, claims);
            var newUser = new AppUser()
            {
                UserName = Guid.NewGuid().ToString(),
                Email = userClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value,
                Active = false
            };

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded) return (null, result);

            result = await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, providerUserId, provider));

            if (!result.Succeeded) return (null, result);

            result = await _userManager.AddClaimsAsync(newUser, userClaims);

            if (!result.Succeeded) return (null, result);

            await _identityService.SendActivationEmailAsync(newUser, Url, Request.Scheme);

            return (newUser, null);
        }

        private IEnumerable<Claim> TransformExternalClaims(string provider, IEnumerable<Claim> claims)
        {
            string email, name, givenName, familyName;

            switch (provider)
            {
                case AuthConstants.AuthSchemes.Facebook:
                    email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    familyName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    break;
                case AuthConstants.AuthSchemes.Google:
                    email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    familyName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    break;
                case AuthConstants.AuthSchemes.Windows:
                    email = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role && c.Value.StartsWith("MicrosoftAccount"))?.Value
                        .Split('\\')[1];
                    name = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;
                    givenName = null; familyName = null;
                    break;
                default:
                    throw new NotSupportedException();
            }

            var userClaims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Name, name),
                new Claim(JwtClaimTypes.Email, email),
                new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean)
            };

            if (givenName != null)
            {
                userClaims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.GivenName, givenName),
                    new Claim(JwtClaimTypes.FamilyName, familyName)
                });
            }

            return userClaims;
        }

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }

        private FillInformationViewModel BuildFillInformationViewModel(IEnumerable<Claim> claims, string returnUrl)
        {
            return new FillInformationViewModel()
            {
                Email = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value,
                FamilyName = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName)?.Value,
                GivenName = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.GivenName)?.Value,
                ReturnUrl = returnUrl
            };
        }
    }
}