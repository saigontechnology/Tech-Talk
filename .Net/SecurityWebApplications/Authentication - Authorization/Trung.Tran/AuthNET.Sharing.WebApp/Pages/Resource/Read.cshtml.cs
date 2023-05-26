using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.Resource
{
    [Authorize(AuthConstants.Policies.CanReadResource)]
    public class ReadModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
