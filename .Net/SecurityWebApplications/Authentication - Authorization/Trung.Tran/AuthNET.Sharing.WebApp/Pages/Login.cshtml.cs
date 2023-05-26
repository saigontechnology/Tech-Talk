using AuthNET.Sharing.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public LoginModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IEnumerable<ExternalProvider> ExternalProviders => new[]
        {
            new ExternalProvider { AuthenticationScheme = AuthConstants.AuthenticationSchemes.Facebook },
            new ExternalProvider { AuthenticationScheme = AuthConstants.AuthenticationSchemes.Google }
        };

        [BindProperty]
        public LoginInputViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _identityService.AuthenticateAsync(Input.Username, Input.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return Page();
            }

            AuthenticationProperties props = null;
            if (Input.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };
            };

            var claimsPrincipal = await _identityService
                .GetUserPrincipalAsync(user, CookieAuthenticationDefaults.AuthenticationScheme);

            // [Important] since Cookies is default, we don't need to specify it explicitly 
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, props);
            await HttpContext.SignInAsync(claimsPrincipal, props);

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }
    }

    public class LoginInputViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }

    public class ExternalProvider
    {
        public string AuthenticationScheme { get; set; }
        public string DisplayName => AuthenticationScheme;
    }
}
