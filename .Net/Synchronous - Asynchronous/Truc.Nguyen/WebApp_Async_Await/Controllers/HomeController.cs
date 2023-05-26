using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SimpleAsyncAwait.WebApp.Models;
using SimpleAsyncAwait.Core.Asynchronous;
using System.Threading;

namespace SimpleAsyncAwait.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CancellationTokenSource cancellationTokenSource;
        private static int timesClick = 0;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {

            return View();
        }

        public async Task ActionAsync()
        {
            //BlockUI.BlockUI1();
            //BlockUI.NonBlockUI3();

            /// Create & Execute Tasks
            //IntroTask.TaskInstantiation();
            //await IntroTask.CommonWaysToCreateNewTask();

            // Popular Methods
            //await PopularMethods.ConfigureAwaitState();

            // Best Practices
            //await BestPractices.AsyncWithException();
            timesClick++;
            await BestPractices.SumTask(5, 10, timesClick);

            timesClick = timesClick == 3 ? 0 : timesClick;

        }

        public async Task TaskWhen()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1500);
            PopularMethods.TaskWhen(cancellationTokenSource);
        }

        public async Task TaskWait()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(1000);
            PopularMethods.TaskWait(cancellationTokenSource);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
