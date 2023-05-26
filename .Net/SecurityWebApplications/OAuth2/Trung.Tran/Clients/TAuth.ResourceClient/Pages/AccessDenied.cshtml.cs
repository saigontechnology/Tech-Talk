using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TAuth.ResourceClient.Pages
{
    public class AccessDeniedModel : PageModel
    {
        [FromQuery]
        public string ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            var hasAccessDeniedFlag = HttpContext.Session.GetInt32("AccessDenied") == 1;

            if (!hasAccessDeniedFlag) return LocalRedirect("/");

            HttpContext.Session.Remove("AccessDenied");

            return Page();
        }
    }
}
