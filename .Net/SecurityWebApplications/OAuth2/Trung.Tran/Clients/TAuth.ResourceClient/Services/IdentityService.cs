using IdentityModel.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TAuth.ResourceClient.Exceptions;

namespace TAuth.ResourceClient.Services
{
    public interface IIdentityService
    {
        Task<IEnumerable<Claim>> GetUserInfoAsync();
    }

    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstants.IdentityAPI);
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync()
        {
            var metadataResp = await _httpClient.GetDiscoveryDocumentAsync();

            if (metadataResp.IsError)
            {
                throw metadataResp.Exception;
            }

            // Alternative: use HttpContext.GetUserAccessTokenAsync() to get access token
            var resp = await _httpClient.GetAsync(metadataResp.UserInfoEndpoint);

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpException()
                {
                    Response = resp
                };
            }

            var json = await resp.Content.ReadFromJsonAsync<JsonElement>();
            return json.ToClaims(issuer: metadataResp.Issuer);
        }

    }
}
