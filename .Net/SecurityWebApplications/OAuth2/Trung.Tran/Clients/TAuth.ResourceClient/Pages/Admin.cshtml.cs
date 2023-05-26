using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TAuth.Resource.Cross;

namespace TAuth.ResourceClient.Pages
{
    [Authorize(Roles = RoleNames.Administrator)]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
