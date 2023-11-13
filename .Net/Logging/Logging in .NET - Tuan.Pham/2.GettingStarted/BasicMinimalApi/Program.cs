var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(x =>
    {
        x.AddConsole();
        x.ClearProviders();
        x.AddFilter(x => x == LogLevel.Information);
    });

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    return "Hello World!";
});

app.Run();
