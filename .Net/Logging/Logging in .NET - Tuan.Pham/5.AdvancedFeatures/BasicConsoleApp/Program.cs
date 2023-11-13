using System.Text.Json;
using BasicConsoleApp;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole(x =>
    {
        x.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
    });
    builder.SetMinimumLevel(LogLevel.Warning);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

var paymentId = 1;
var amount = 15.99;

while (true)
{
    logger.LogInformation(
        "New Payment with id {PaymentId} for ${Total}", paymentId, amount);
    await Task.Delay(1000);
}


