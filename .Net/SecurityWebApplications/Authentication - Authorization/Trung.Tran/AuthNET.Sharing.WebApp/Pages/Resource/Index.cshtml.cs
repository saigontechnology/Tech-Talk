using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Pages.Resource
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public IndexModel(DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public IEnumerable<AppResourceModel> Resources { get; set; }

        public void OnGet()
        {
            IQueryable<AppResource> resourceQuery = _dataContext.Resources;

            if (!User.IsInRole(AuthConstants.RoleNames.Administrator))
            {
                resourceQuery = resourceQuery.Where(r => r.UserId == User.Identity.Name);
            }

            Resources = resourceQuery
                .ProjectTo<AppResourceModel>(_mapper.ConfigurationProvider)
                .ToArray();
        }
    }
}
