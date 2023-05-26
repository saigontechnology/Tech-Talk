using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.RoleBased
{
    [Authorize(Roles = AuthConstants.RoleNames.Administrator)]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
