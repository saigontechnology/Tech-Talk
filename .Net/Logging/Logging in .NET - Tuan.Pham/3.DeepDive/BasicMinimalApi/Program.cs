var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/test", (ILogger<Program> logger) =>
{
    logger.LogInformation("test");
});

app.Run();
