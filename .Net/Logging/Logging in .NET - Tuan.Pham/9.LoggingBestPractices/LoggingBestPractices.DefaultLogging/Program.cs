using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
});

ILogger logger = new Logger<Program>(loggerFactory);

for (var i = 0; i < 69_000_000; i++)
{
    // if (logger.IsEnabled(LogLevel.Information))
    // {
        logger.LogInformation("Random number {RandomNumber}", Random.Shared.Next());
    // }
}
