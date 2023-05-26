using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.RoleBased
{
    [Authorize(Roles = AuthConstants.RoleNames.Employee)]
    public class EmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
