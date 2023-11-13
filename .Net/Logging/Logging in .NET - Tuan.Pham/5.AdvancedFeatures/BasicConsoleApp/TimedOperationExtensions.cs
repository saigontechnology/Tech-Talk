using Microsoft.Extensions.Logging;

namespace BasicConsoleApp;

public static class TimedOperationExtensions
{
    public static IDisposable BeginTimedOperation(
        this ILogger logger, string messageTemplate, params object[] args)
    {
        return BeginTimedOperation(logger, LogLevel.Information, messageTemplate, args);
    }
    
    public static IDisposable BeginTimedOperation(
        this ILogger logger, LogLevel logLevel, string messageTemplate, params object[] args)
    {
        return new TimedOperation(logger, logLevel, messageTemplate, args);
    }
}
