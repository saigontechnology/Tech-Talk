using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Handlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public BearerTokenHandler(IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var expiresAt = await httpContext.GetTokenAsync("expires_at");
            var expiresAtTime = DateTimeOffset.Parse(expiresAt, CultureInfo.InvariantCulture);

            if (expiresAtTime.AddSeconds(-60) > DateTimeOffset.UtcNow)
            {
                return await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }

            var refreshToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //var refreshResp = await _identityService.RefreshTokenAsync(refreshToken);

            // [TODO] Update tokens. Notes: use IdentityModel.AspNetCore libs instead

            //return refreshResp.AccessToken;

            return default;
        }
    }
}
