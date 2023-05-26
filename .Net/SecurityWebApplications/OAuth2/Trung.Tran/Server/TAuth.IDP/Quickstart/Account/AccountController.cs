// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TAuth.Cross.Services;
using TAuth.IDP;
using TAuth.IDP.Models;
using TAuth.IDP.Services;

namespace IdentityServerHost.Quickstart.UI
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private const string AuthenticatorSecretDictionaryKey = "AuthenticatorSecret";

        private readonly IdpContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public AccountController(
            IdpContext dbContext,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IEmailService emailService,
            IIdentityService identityService,
            UserManager<AppUser> userManager)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            //_users = users ?? new TestUserStore(TestUsers.Users);

            _dbContext = dbContext;
            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _emailService = emailService;
            _identityService = identityService;
            _events = events;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var validLogin = false;
                var user = await _userManager.FindByNameAsync(model.Username);

                // validate username/password against identity store
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    validLogin = user.Active;

                    if (!user.Active)
                    {
                        await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "email not confirmed", clientId: context?.Client.ClientId));
                        ModelState.AddModelError(string.Empty, AccountOptions.EmailNotConfirmedErrorMessage);
                    }
                }
                else
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }

                if (validLogin)
                {
                    if (!Startup.AppSettings.EnableMfa)
                    {
                        return await LoginAsync(user, model.ReturnUrl, model.RememberLogin);
                    }

                    var mfaIdentity = new ClaimsIdentity(AuthConstants.AuthSchemes.IdentityMfa);
                    mfaIdentity.AddClaim(new Claim(JwtClaimTypes.Subject, user.Id));
                    var mfaPrincipal = new ClaimsPrincipal(mfaIdentity);
                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AuthConstants.Mfa.DefaultExpireTime),
                        IsPersistent = true
                    };

                    // demo only: in real scenarios, should use email as backup OTP and change to soft OTP only when user has set it up
                    if (Startup.AppSettings.UseAuthenticatorApp)
                    {
                        await HttpContext.SignInAsync(AuthConstants.AuthSchemes.IdentityMfa, mfaPrincipal, props);

                        var hasSecret = _dbContext.UserSecrets.Any(secret => secret.UserId == user.Id
                            && secret.Name == AuthConstants.Mfa.OTPSecretKeyName);

                        if (!hasSecret)
                        {
                            var secretKey = TwoStepsAuthenticator.Authenticator.GenerateKey(AuthConstants.Mfa.AuthenticatorAppSecretKeyLength);
                            var setupInfo = Startup.GoogleAuthenticator.GenerateSetupCode(AuthConstants.IDPName, user.Email, secretKey, true);

                            return View("SetupOTPApp", new SetupOTPAppViewModel
                            {
                                QrCodeSetupImageUrl = setupInfo.QrCodeSetupImageUrl,
                                RememberLogin = model.RememberLogin,
                                ReturnUrl = model.ReturnUrl,
                                SecretKey = secretKey
                            });
                        }
                    }
                    else
                    {
                        var authenticatorKey = TwoStepsAuthenticator.Authenticator.GenerateKey();
                        // Note: can not used for Load balancer since Authenticator is memory-based by default => must implement persistence instead
                        var totp = Startup.Authenticator.GetCode(authenticatorKey);

                        await _emailService.SendEmailAsync(user.Email, "Check your OTP to login", $"Your OTP is: {totp}");

                        // Demo only: should store secret instead of generating new one everytime
                        props.Items[AuthenticatorSecretDictionaryKey] = authenticatorKey;

                        await HttpContext.SignInAsync(AuthConstants.AuthSchemes.IdentityMfa, mfaPrincipal, props);
                    }

                    return RedirectToAction(nameof(CheckOTP), new
                    {
                        model.RememberLogin,
                        returnUrl = model.ReturnUrl
                    });
                }
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        #region Direct Login
#if false
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var validLogin = false;
                var user = await _userManager.FindByNameAsync(model.Username);

                // validate username/password against identity store
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    validLogin = user.Active;

                    if (!user.Active)
                    {
                        await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "email not confirmed", clientId: context?.Client.ClientId));
                        ModelState.AddModelError(string.Empty, AccountOptions.EmailNotConfirmedErrorMessage);
                    }
                }
                else
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }

                if (validLogin)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    var isuser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName
                    };

                    await HttpContext.SignInAsync(isuser, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }
#endif
        #endregion

        /// <summary>
        /// Show OTP confirm page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CheckOTP(bool rememberLogin, string returnUrl)
        {
            var mfaAuthResult = await HttpContext.AuthenticateAsync(AuthConstants.AuthSchemes.IdentityMfa);

            if (!mfaAuthResult.Succeeded) return RedirectToAction(nameof(Login));

            var vm = BuildCheckOTPViewModel(rememberLogin, returnUrl);

            return View(vm);
        }

        /// <summary>
        /// Check inputted OTP
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CheckOTP(CheckOTPViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // something went wrong, show form with error
                return View(viewModel);
            }

            var mfaAuthResult = await HttpContext.AuthenticateAsync(AuthConstants.AuthSchemes.IdentityMfa);

            if (!mfaAuthResult.Succeeded) return RedirectToAction(nameof(Login));

            var subject = mfaAuthResult.Principal.FindFirstValue(JwtClaimTypes.Subject);
            var user = await _userManager.FindByIdAsync(subject);

            if (user == null) return NotFound();

            if (Startup.AppSettings.UseAuthenticatorApp)
            {
                var totpUserSecret = await _dbContext.UserSecrets.Where(s => s.UserId == subject && s.Name == AuthConstants.Mfa.OTPSecretKeyName)
                    .Select(s => s.Secret).FirstOrDefaultAsync();
                var isValidOTP = Startup.GoogleAuthenticator.ValidateTwoFactorPIN(totpUserSecret, viewModel.OTPCode, TimeSpan.Zero, true);

                if (!isValidOTP)
                {
                    ModelState.AddModelError("", "Invalid OTP");
                    return View(viewModel);
                }
            }
            else
            {
                var authenticatorKey = mfaAuthResult.Properties.Items[AuthenticatorSecretDictionaryKey];
                var isValidOTP = Startup.Authenticator.CheckCode(authenticatorKey, viewModel.OTPCode, user);

                if (!isValidOTP)
                {
                    ModelState.AddModelError("", "Invalid OTP");
                    return View(viewModel);
                }
            }

            return await LoginAsync(user, viewModel.ReturnUrl, viewModel.RememberLogin);
        }

        /// <summary>
        /// Handle setup OTP app
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SetupOTPApp(SetupOTPAppViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // something went wrong, show form with error
                return View(viewModel);
            }

            var mfaAuthResult = await HttpContext.AuthenticateAsync(AuthConstants.AuthSchemes.IdentityMfa);

            if (!mfaAuthResult.Succeeded) return RedirectToAction(nameof(Login));

            var subject = mfaAuthResult.Principal.FindFirstValue(JwtClaimTypes.Subject);

            _dbContext.UserSecrets.Add(new UserSecret
            {
                Name = AuthConstants.Mfa.OTPSecretKeyName,
                Secret = viewModel.SecretKey,
                UserId = subject
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(CheckOTP), new
            {
                rememberLogin = viewModel.RememberLogin,
                returnUrl = viewModel.ReturnUrl
            });
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = BuildRegisterViewModel(returnUrl);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> FillInformation(FillInformationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // read external identity from the temporary cookie
            var externalAuthResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (externalAuthResult?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            var externalUser = externalAuthResult.Principal;
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");
            var provider = externalAuthResult.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            if (user != null) return BadRequest();

            user = new AppUser
            {
                UserName = Guid.NewGuid().ToString(),
                Email = viewModel.Email,
                Active = false
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                SetIdentityResultErrors(result);
                return View(viewModel);
            }

            result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

            if (!result.Succeeded)
            {
                SetIdentityResultErrors(result);
                return View(viewModel);
            }

            result = await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim(JwtClaimTypes.Name, $"{viewModel.FamilyName} {viewModel.GivenName}"),
                new Claim(JwtClaimTypes.GivenName, viewModel.GivenName),
                new Claim(JwtClaimTypes.FamilyName, viewModel.FamilyName),
                new Claim(JwtClaimTypes.Email, viewModel.Email),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, $"http://{viewModel.Email}.com"),
                new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(new
                {
                    street_address = viewModel.Address,
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = viewModel.Country
                }), IdentityServerConstants.ClaimValueTypes.Json)
            });

            if (!result.Succeeded)
            {
                SetIdentityResultErrors(result);
                return View(viewModel);
            }

            await _identityService.SendActivationEmailAsync(user, Url, Request.Scheme);

            return View("Message", new MessageViewModel { Message = "Please confirm your email before login" });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new AppUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Active = false
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                SetIdentityResultErrors(result);
                return View(viewModel);
            }

            result = await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim(JwtClaimTypes.Name, $"{viewModel.FamilyName} {viewModel.GivenName}"),
                new Claim(JwtClaimTypes.GivenName, viewModel.GivenName),
                new Claim(JwtClaimTypes.FamilyName, viewModel.FamilyName),
                new Claim(JwtClaimTypes.Email, viewModel.Email),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, $"http://{viewModel.UserName}.com"),
                new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(new
                {
                    street_address = viewModel.Address,
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = viewModel.Country
                }), IdentityServerConstants.ClaimValueTypes.Json)
            });

            if (!result.Succeeded)
            {
                SetIdentityResultErrors(result);
                return View(viewModel);
            }

            await _identityService.SendActivationEmailAsync(user, Url, Request.Scheme);

            return View("Message", new MessageViewModel { Message = "Please confirm your email before login" });

            // issue authentication cookie with subject ID and username
            //var isuser = new IdentityServerUser(user.Id)
            //{
            //    DisplayName = user.UserName
            //};

            //await HttpContext.SignInAsync(isuser);

            //if (_interaction.IsValidReturnUrl(viewModel.ReturnUrl)
            //    || Url.IsLocalUrl(viewModel.ReturnUrl))
            //{
            //    return Redirect(viewModel.ReturnUrl);
            //}

            //return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.Active = true;

                result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    SetIdentityResultErrors(result);
                    return View("Message", new MessageViewModel());
                }

                result = await _userManager.ReplaceClaimAsync(user,
                    new Claim(JwtClaimTypes.EmailVerified, "false"),
                    new Claim(JwtClaimTypes.EmailVerified, "true"));

                if (!result.Succeeded)
                {
                    SetIdentityResultErrors(result);
                    return View("Message", new MessageViewModel());
                }

                result = await _userManager.UpdateSecurityStampAsync(user);

                if (!result.Succeeded)
                {
                    SetIdentityResultErrors(result);
                    return View("Message", new MessageViewModel());
                }

                return View("Message", new MessageViewModel { Message = "Confirmed email successfully!" });
            }

            SetIdentityResultErrors(result);
            return View("Message", new MessageViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId = null, string token = null)
        {
            AppUser user = null;

            if (userId != null)
            {
                user = await _userManager.FindByIdAsync(userId);

                if (user == null) return NotFound();
            }

            var vm = BuildResetPasswordViewModel(user, token);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RequestResetPassword(RequestResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var vm = BuildResetPasswordViewModel(requestResetPasswordViewModel: viewModel);

                return View(nameof(ResetPassword), vm);
            }

            var user = await _userManager.FindByNameAsync(viewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Not found user");

                var vm = BuildResetPasswordViewModel(requestResetPasswordViewModel: viewModel);

                return View(nameof(ResetPassword), vm);
            }

            // Default: 1 day timespan for token valid lifetime => need resend function
            var confirmToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action(
               nameof(ResetPassword), "Account",
               new { userId = user.Id, token = confirmToken },
               protocol: Request.Scheme);

            await _emailService.SendEmailAsync(user.Email,
               "Reset your password",
               $"You can reset your password by clicking this <a href=\"{callbackUrl}\">link</a>");

            return View("Message", new MessageViewModel { Message = "An email has been sent." });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByIdAsync(viewModel.UserId);

            if (user == null) return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, viewModel.ResetPasswordToken, viewModel.Password);

            if (result.Succeeded)
            {
                return View("Message", new MessageViewModel { Message = "Reset password successfully!" });
            }

            SetIdentityResultErrors(result);
            return View("Message", new MessageViewModel());
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<IActionResult> LoginAsync(AppUser user, string returnUrl, bool rememberLogin)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (AccountOptions.AllowRememberLogin && rememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(user.Id)
            {
                DisplayName = user.UserName
            };

            await HttpContext.SignInAsync(isuser, props);

            if (Startup.AppSettings.EnableMfa) await HttpContext.SignOutAsync(AuthConstants.AuthSchemes.IdentityMfa);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnUrl);
                }

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(returnUrl);
            }

            // request for a local page
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }

        private ResetPasswordViewModel BuildResetPasswordViewModel(AppUser user = null,
            string token = null,
            RequestResetPasswordViewModel requestResetPasswordViewModel = null)
        {
            var viewModel = new ResetPasswordViewModel();

            if (user != null)
            {
                viewModel.UserId = user?.Id;
                viewModel.ResetPasswordToken = token;
                viewModel.IdentityConfirmed = true;
            }
            else
            {
                viewModel.RequestResetPasswordViewModel = requestResetPasswordViewModel ?? new RequestResetPasswordViewModel();
            }

            return viewModel;
        }

        private RegisterViewModel BuildRegisterViewModel(string returnUrl)
        {
            return new RegisterViewModel()
            {
                ReturnUrl = returnUrl
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private CheckOTPViewModel BuildCheckOTPViewModel(bool rememberLogin, string returnUrl)
        {
            return new CheckOTPViewModel
            {
                RememberLogin = rememberLogin,
                ReturnUrl = returnUrl
            };
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            // comment for disabling automatically sign-out
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
