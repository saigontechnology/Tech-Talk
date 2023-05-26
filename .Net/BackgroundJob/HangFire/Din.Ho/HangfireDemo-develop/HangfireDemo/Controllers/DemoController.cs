using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        [Route("DemoFireAndForgetJob")]
        public void DemoFireAndForgetJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
        }

        [HttpGet]
        [Route("DemoDelayedJob")]
        public void DemoDelayedJob()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromSeconds(5));
        }

        [HttpGet]
        [Route("DemoContinuationsJob")]
        public void DemoContinuationsJob()
        {
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Main Job"), TimeSpan.FromSeconds(5));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Dependent Job"));
        }
    }
}
