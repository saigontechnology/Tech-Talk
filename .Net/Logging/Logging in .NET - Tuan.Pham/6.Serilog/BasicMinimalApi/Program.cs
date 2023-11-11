using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

Serilog.ILogger logger = new LoggerConfiguration()
    .WriteTo.Console(theme:AnsiConsoleTheme.Code)
    // .WriteTo.Console(new CompactJsonFormatter())
    // .Enrich.WithThreadId()
    .CreateLogger();

Log.Logger = logger;

builder.Services.AddSingleton(logger);

var app = builder.Build();

app.MapGet("/", (Serilog.ILogger log) =>
{
    log.Information("Hello from the endpoint");
    return "Hello World!";
});

app.Run();
