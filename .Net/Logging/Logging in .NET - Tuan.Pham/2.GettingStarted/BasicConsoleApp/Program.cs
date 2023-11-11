using BasicConsoleApp;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Information)
        .AddConsole();
    //.AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();

var name = "Tuan";
var age = 24;

//logger.LogInformation("{Name} just turned: {Age}", name, age);

logger.LogInformation(5001, "{Name} just turned: {Age}", name, age);





