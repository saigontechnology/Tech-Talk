using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet(string returnUrl)
        {
            if (returnUrl != null && !Url.IsLocalUrl(returnUrl)) return BadRequest();

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync();

            if (returnUrl == null) return LocalRedirect("/");

            return new EmptyResult();
        }
    }
}
