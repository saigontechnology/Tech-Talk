using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeadlockController : ControllerBase
    {
        [Route("Demo1")]
        [HttpGet]
        public async Task<int> DownloadStringV1()
        {
            // good code
            var client = new HttpClient();
            var request = await client.GetAsync("https://localhost:7280/api/greetings");
            var getStringTask = request.Content.ReadAsStringAsync();
            string contents = getStringTask.Result;
            return contents.Length;
        }

        [Route("Demo2")]
        [HttpGet]
        public async Task<int> DownloadStringV2()
        {
            var client = new HttpClient();
            var request = client.GetAsync("https://localhost:7280/api/greetings").Result;
            var contents = request.Content.ReadAsStringAsync().Result;
            return contents.Length;
        }

    }
}
