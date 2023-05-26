using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.Resource;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IResourceService _resourceService;

        public CreateModel(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [BindProperty]
        public CreateResourceModel RequestModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = await _resourceService.CreateAsync(RequestModel);

            return RedirectToPage("/Index");
        }
    }
}
