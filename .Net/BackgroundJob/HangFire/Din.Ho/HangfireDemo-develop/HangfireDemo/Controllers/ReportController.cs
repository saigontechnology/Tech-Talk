using Hangfire;
using HangfireDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("GenerateReport")]
        public Task GenerateReport()
        {
            return _reportService.GenerateReport();
        }

        [HttpGet]
        [Route("GenerateReportNoWait")]
        public void GenerateReportNoWait()
        {
            BackgroundJob.Enqueue(() => _reportService.GenerateReportNoWait());
            //BackgroundJob.Enqueue<ReportService>(x => x.GenerateReportNoWait());
        }
    }
}
