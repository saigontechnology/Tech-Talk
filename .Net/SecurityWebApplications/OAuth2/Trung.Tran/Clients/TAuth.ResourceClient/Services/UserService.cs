using IdentityModel.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.User;
using TAuth.ResourceClient.Exceptions;

namespace TAuth.ResourceClient.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfileItem>> GetUserProfileAsync(string accessToken);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstants.ResourceAPI);
        }

        public async Task<IEnumerable<UserProfileItem>> GetUserProfileAsync(string accessToken)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"/api/users/profile");
            message.SetBearerToken(accessToken);

            var resp = await _httpClient.SendAsync(message);

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpException()
                {
                    Response = resp
                };
            }

            return await resp.Content.ReadFromJsonAsync<IEnumerable<UserProfileItem>>();
        }
    }
}
