using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.Auth
{
    [Authorize(AuthConstants.Policies.SingleRoleOnly)]
    public class AssertionModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
