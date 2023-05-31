using DataLayer.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoCQRS
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception error)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case MissingHandlerException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    case NotImplementedException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    case IdentityNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
