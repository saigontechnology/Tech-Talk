using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public IndexModel(ILogger<IndexModel> logger,
            DataContext dataContext,
            IMapper mapper)
        {
            _logger = logger;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public IEnumerable<UserClaimModel> UserClaims { get; set; }
        public AppUserModel UserInfo { get; set; }
        public string UserInfoSerialized { get; set; }

        public void OnGet()
        {
            UserClaims = User.Claims.Select(claim => new UserClaimModel
            {
                Type = claim.Type,
                Value = claim.Value
            }).ToArray();

            UserInfo = _dataContext.Users.Where(user => user.Id == User.Identity.Name)
                .ProjectTo<AppUserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (UserInfo != null)
                UserInfoSerialized = JsonConvert.SerializeObject(UserInfo, Formatting.Indented);
            else UserInfoSerialized = "User is not in database yet";
        }
    }

    public class UserClaimModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
