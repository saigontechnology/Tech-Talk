using AuthNET.Sharing.WebApp.Auth.Policies;
using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Pages.Resource
{
    [BindProperties(SupportsGet = true)]
    public class ManageModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public ManageModel(DataContext dataContext,
            IAuthorizationService authorizationService,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        public Guid ResourceId { get; set; }
        public AppResourceModel Resource { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Resource = _dataContext.Resources
                .Where(r => r.Id == ResourceId)
                .ProjectTo<AppResourceModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (Resource == null)
            {
                ModelState.AddModelError("", "Not found resource");
                return Page();
            }

            var canManageResource = (await _authorizationService.AuthorizeAsync(
                User, new CanManageResourceContext
                {
                    ResourceId = Resource.Id
                }, AuthConstants.Policies.CanManageResource)).Succeeded;

            if (!canManageResource)
            {
                return Forbid();
            }

            return Page();
        }
    }
}
