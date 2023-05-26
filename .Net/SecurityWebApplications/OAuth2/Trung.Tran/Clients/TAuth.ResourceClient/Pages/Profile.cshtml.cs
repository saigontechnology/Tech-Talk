using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceClient.Auth.Policies;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Pages
{
    [Authorize(PolicyNames.EmailVerified)]
    public class ProfileModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public ProfileModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public string Address { get; set; } = "Unknown";
        public string FullName { get; set; }

        public async Task OnGetAsync()
        {
            var userInfo = await _identityService.GetUserInfoAsync();

            FullName = userInfo.FirstOrDefault(o => o.Type == JwtClaimTypes.Name)?.Value;

            var addrVal = userInfo.FirstOrDefault(o => o.Type == JwtClaimTypes.Address)?.Value;
            if (!string.IsNullOrWhiteSpace(addrVal))
            {
                var addressObj = JsonConvert.DeserializeAnonymousType(addrVal, new
                {
                    street_address = "",
                    locality = "",
                    postal_code = 0,
                    country = ""
                });

                Address = string.Join(", ", addressObj.street_address, addressObj.locality, $"{addressObj.postal_code}", addressObj.country);
            }
        }
    }
}
