using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TAuth.ResourceClient.Exceptions;

namespace TAuth.ResourceClient.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet() => HandleError();

        public IActionResult OnPost() => HandleError();

        private IActionResult HandleError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionFeature?.Error;

            if (ex is HttpException httpEx)
            {
                if (httpEx.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return Challenge();

                if (httpEx.Response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return Forbid();
            }

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return Page();
        }
    }
}
