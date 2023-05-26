using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthNET.Sharing.WebApp.Pages.Resource
{
    // [Important] can only applied to PageModel only, can not applied on handlers
    [Authorize(AuthConstants.Policies.CanWriteResource)]
    public class WriteModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
