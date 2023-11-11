using BasicConsoleApp;
using Destructurama;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using SerilogTimings.Extensions;

ILogger logger = new LoggerConfiguration()
    .WriteTo.Async(x => x.Console(theme:AnsiConsoleTheme.Code), 10)
    //.WriteTo.Console(new JsonFormatter())
    .Enrich.FromLogContext()
    .Destructure.UsingAttributes()
    .CreateLogger();

Log.Logger = logger;

var payment = new Payment
{
    PaymentId = 1,
    Email = "tuan@saigon.com",
    UserId = Guid.NewGuid(),
    OccuredAt = DateTime.UtcNow
};

logger.Information("Received payment with details {@PaymentData}", payment);


using (LogContext.PushProperty("PaymentId", payment.PaymentId))
{
    logger.Information("Received payment with {Email}", payment.Email);
}



Log.CloseAndFlush();



