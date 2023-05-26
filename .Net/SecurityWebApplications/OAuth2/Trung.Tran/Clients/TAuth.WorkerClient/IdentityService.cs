using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace TAuth.WorkerClient
{
    public interface IIdentityService
    {
        Task<TokenRevocationResponse> RevokeTokenAsync(string clientId, string clientSecret, string token);
    }

    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenRevocationResponse> RevokeTokenAsync(string clientId, string clientSecret, string token)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync();

            var revokeResp = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = disco.RevocationEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Token = token
            });

            return revokeResp;
        }
    }
}
