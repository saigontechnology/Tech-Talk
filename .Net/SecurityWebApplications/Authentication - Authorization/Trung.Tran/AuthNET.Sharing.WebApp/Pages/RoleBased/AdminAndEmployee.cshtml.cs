using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.RoleBased
{
    [Authorize(Roles = AuthConstants.RoleNames.Administrator)]
    [Authorize(Roles = AuthConstants.RoleNames.Employee)]
    public class AdminAndEmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
