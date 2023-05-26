using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.Resource;
using TAuth.ResourceClient.Auth.Policies;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IResourceService _resourceService;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(ILogger<IndexModel> logger,
            IResourceService resourceService,
            IAuthorizationService authorizationService)
        {
            _logger = logger;
            _resourceService = resourceService;
            _authorizationService = authorizationService;
        }

        public IEnumerable<ResourceListItemModel> ResourceList { get; set; }
        public bool CanCreateResource { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await DebugIdentity();

            ResourceList = await _resourceService.GetAsync();
            CanCreateResource = (await _authorizationService.AuthorizeAsync(User, PolicyNames.Resource.CanCreateResource)).Succeeded;

            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            await _resourceService.DeleteAsync(id);

            return RedirectToPage("/Index");
        }

        private async Task DebugIdentity()
        {
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accessToken = await HttpContext.GetUserAccessTokenAsync();
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            Console.WriteLine(refreshToken);
            Debug.WriteLine($"IdToken: {idToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }
        }
    }
}
