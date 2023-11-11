using Microsoft.Extensions.Logging;

#region Configuration

using var loggerFactory = LoggerFactory.Create(builder =>
{
#pragma warning disable CA1416
    builder
        .AddJsonConsole();
        //.AddEventLog();
#pragma warning restore CA1416
});

ILogger<Program> CreateLogger()
{
    return loggerFactory.CreateLogger<Program>();
}
#endregion

ILogger<Program> logger = CreateLogger();

var name = "Tuan";

logger.LogInformation($"Hello from {name}!");

logger.LogInformation("Hello from {name}!", name);

var tuan1 = "Tuan";
var tuan2 = "Tuan";

Console.WriteLine(tuan1 == tuan2);

