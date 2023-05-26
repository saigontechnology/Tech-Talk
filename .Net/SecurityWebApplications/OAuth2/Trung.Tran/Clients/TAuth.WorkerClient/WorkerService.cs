using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TAuth.WorkerClient
{
    public interface IWorkerService
    {
        Task StartAsync();
    }

    public class WorkerService : IWorkerService
    {
        private readonly HttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public WorkerService(HttpClient httpClient,
            IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                Console.Write("Input or leave blank: ");
                var revokeRequest = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(revokeRequest))
                {
                    var parts = revokeRequest.Split('|');

                    var revokeResp = await _identityService.RevokeTokenAsync(parts[0], parts[1], parts[2]);

                    if (!revokeResp.IsError)
                        Console.WriteLine(revokeResp.HttpResponse);
                    else Console.WriteLine("Failed to revoke token");

                    continue;
                }

                var resp = await _httpClient.GetAsync("/api/background");

                if (resp.IsSuccessStatusCode)
                {
                    var content = await resp.Content.ReadFromJsonAsync<JsonElement>();
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(resp);
                    Console.WriteLine(resp.StatusCode);
                }

                Thread.Sleep(5000);
            }
        }
    }
}
