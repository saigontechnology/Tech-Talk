using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.Resource;
using TAuth.ResourceClient.Exceptions;

namespace TAuth.ResourceClient.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceListItemModel>> GetAsync();
        Task<ResourceDetailModel> GetAsync(int id);
        Task<int> CreateAsync(CreateResourceModel model);
        Task DeleteAsync(int id);
    }

    public class ResourceService : IResourceService
    {
        private readonly HttpClient _httpClient;

        public ResourceService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstants.ResourceAPI);
        }

        public async Task<int> CreateAsync(CreateResourceModel model)
        {
            var resp = await _httpClient.PostAsJsonAsync("/api/resources", model);

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpException()
                {
                    Response = resp
                };
            }

            var createdId = await resp.Content.ReadFromJsonAsync<int>();
            return createdId;
        }

        public async Task DeleteAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync($"/api/resources/{id}");

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpException()
                {
                    Response = resp
                };
            }
        }

        public async Task<IEnumerable<ResourceListItemModel>> GetAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, "/api/resources");
            var resp = await _httpClient.SendAsync(message);

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpException()
                {
                    Response = resp
                };
            }

            return await resp.Content.ReadFromJsonAsync<IEnumerable<ResourceListItemModel>>();
        }

        public async Task<ResourceDetailModel> GetAsync(int id)
        {
            var item = await _httpClient.GetFromJsonAsync<ResourceDetailModel>($"/api/resources/{id}");
            return item;
        }
    }
}
