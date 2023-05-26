using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using TAuth.Cross;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Options
{
    public class OpenIdConnectOptionsPostConfigureOptions : IPostConfigureOptions<OpenIdConnectOptions>
    {
        private readonly IUserService _userService;

        public OpenIdConnectOptionsPostConfigureOptions(IUserService userService)
        {
            _userService = userService;
        }

        public void PostConfigure(string name, OpenIdConnectOptions options)
        {
            options.Events = new OpenIdConnectEvents
            {
                OnTicketReceived = async tokenReceivedContext =>
                {
                    var accessToken = tokenReceivedContext.Properties.GetTokenValue(OpenIdConnectConstants.PropertyNames.AccessToken);
                    var userProfileItems = await _userService.GetUserProfileAsync(accessToken);
                    var userClaims = userProfileItems.Select(pi => new Claim(pi.Type, pi.Value)).ToArray();
                    var identity = new ClaimsIdentity(userClaims, authenticationType: null, JwtClaimTypes.Name, JwtClaimTypes.Role);
                    tokenReceivedContext.Principal.AddIdentity(identity);
                }
            };
        }
    }
}
