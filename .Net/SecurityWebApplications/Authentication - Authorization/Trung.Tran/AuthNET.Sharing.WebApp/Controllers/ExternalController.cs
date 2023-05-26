using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        public ExternalController()
        {
        }

        /// <summary>
        /// Initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            if (!Url.IsLocalUrl(returnUrl))
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };

            return Challenge(props, scheme);
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(AuthConstants.AuthenticationSchemes.ExternalCookies);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // [Important] demo only, need to use Identity federation and user provisioning instead
            var externalUser = result.Principal;
            var userIdClaim = externalUser.FindFirst(AuthConstants.AppClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;
            var userClaims = TransformExternalClaims(provider, claims);

            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, providerUserId));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);
            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(AuthConstants.AuthenticationSchemes.ExternalCookies);

            return Redirect(returnUrl);
        }

        private IEnumerable<Claim> TransformExternalClaims(string provider, IEnumerable<Claim> claims)
        {
            string email, name, givenName, familyName;

            switch (provider)
            {
                case AuthConstants.AuthenticationSchemes.Facebook:
                    email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    familyName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    break;
                case AuthConstants.AuthenticationSchemes.Google:
                    email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    familyName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    break;
                default:
                    throw new NotSupportedException();
            }

            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
            };

            if (givenName != null)
            {
                userClaims.AddRange(new[]
                {
                    new Claim(ClaimTypes.GivenName, givenName),
                    new Claim(ClaimTypes.Surname, familyName)
                });
            }

            return userClaims;
        }
    }
}