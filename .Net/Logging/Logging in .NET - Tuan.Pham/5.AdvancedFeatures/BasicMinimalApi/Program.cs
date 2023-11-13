using BasicMinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<LogBackgroundService>();

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    return "Hello Log!";
});

app.Run();
