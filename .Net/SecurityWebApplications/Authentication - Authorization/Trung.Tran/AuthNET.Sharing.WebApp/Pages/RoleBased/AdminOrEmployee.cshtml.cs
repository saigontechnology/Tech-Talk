using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.RoleBased
{
    [Authorize(Roles = AuthConstants.RoleNames.Employee + ","
        + AuthConstants.RoleNames.Administrator)]
    public class AdminOrEmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
