using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        //.SetMinimumLevel(LogLevel.Debug)
        .AddJsonConsole(options =>
        {
            options.TimestampFormat = "HH:mm:ss";
            options.JsonWriterOptions = new JsonWriterOptions()
            {
                Indented = true
            };
            options.IncludeScopes = true;
        });
    // builder.AddFilter(((provider, category, logLevel) =>
    // {
    //     return provider!.Contains("Console")
    //            && category!.Contains("Microsoft.AspNetCore")
    //            && logLevel >= LogLevel.Debug;
    // }));
    builder.AddFilter("Default", LogLevel.Debug)
        .AddFilter<ConsoleLoggerProvider>("Microsoft.AspNetCore", LogLevel.Information);
});

var logger = loggerFactory.CreateLogger<Program>();

var name = "Tuan";
var age = 24;

// try
// {
//     throw new Exception("Something went wrong");
// }
// catch (Exception ex)
// {
//     logger.LogError(ex, "Failure during birthday of {Name} who is {Age}", name, age);
// }

var paymentId = 1;
var amount = 20000;

using (logger.BeginScope("{paymentId}", paymentId))
{
    try
    {
        logger.LogInformation("New payment for {amount}VND", amount);
        //processing
    }
    finally
    {
        logger.LogInformation("Payment processing completed");
    }
}