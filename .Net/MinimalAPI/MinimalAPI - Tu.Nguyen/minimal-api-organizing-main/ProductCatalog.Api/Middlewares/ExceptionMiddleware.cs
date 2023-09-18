using System.Net;
using System.Text.Json;
using ProductCatalog.Api.Exceptions;

namespace ProductCatalog.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.RequestAborted.IsCancellationRequested &&
                (ex is TaskCanceledException || ex is OperationCanceledException))
            {
                _logger.LogInformation(ex, ex.Message);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        object result = null;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = validationException.Failures.SelectMany(f => f.Value);
                break;
            case BadRequestException badRequestException:
                code = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;
            case WrongUserNameOrPassword wrongUserNameOrPassword:
                code = HttpStatusCode.BadRequest;
                result = wrongUserNameOrPassword.Message;
                break;
            case UserNotFoundException userNotFoundException:
                code = HttpStatusCode.NotFound;
                result = userNotFoundException.Message;
                break;
            case UserNameExistsException userNameExistsException:
                code = HttpStatusCode.BadRequest;
                result = userNameExistsException.Message;
                break;
            case ProductNameExistsException productNameExistsException:
                code = HttpStatusCode.BadRequest;
                result = productNameExistsException.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) code;

        return context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = result ?? exception.Message
        }));
    }
}