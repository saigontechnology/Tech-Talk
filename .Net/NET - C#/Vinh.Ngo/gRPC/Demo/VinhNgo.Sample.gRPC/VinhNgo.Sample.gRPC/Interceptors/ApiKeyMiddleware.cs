namespace VinhNgo.Sample.gRPC.Interceptors;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string APIKEYNAME = "X-API-KEY";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

        var apiKey = appSettings.GetValue<string>(APIKEYNAME);

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await next(context);
    }
}