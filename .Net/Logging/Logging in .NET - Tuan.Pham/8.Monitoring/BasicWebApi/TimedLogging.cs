using System.Diagnostics;

namespace BasicWebApi;

public static class TimedLogging
{
    public static IDisposable BeginTimedOperation(
        this ILogger logger, string messageTemplate, params object?[] args)
    {
        return BeginTimedOperation(logger, LogLevel.Information, messageTemplate, args);
    }
    
    public static IDisposable BeginTimedOperation(
        this ILogger logger, LogLevel logLevel, string messageTemplate, params object?[] args)
    {
        return new TimedLogOperation(logger, logLevel, messageTemplate, args);
    }
}

public class TimedLogOperation : IDisposable
{
    private readonly ILogger _logger;
    private readonly LogLevel _logLevel;
    private readonly string _message;
    private readonly object?[] _args;
    private readonly long _startTimestamp;

    public TimedLogOperation(
        ILogger logger, LogLevel logLevel,
        string message, object?[] args)
    {
        _logger = logger;
        _logLevel = logLevel;
        _message = message;
        _args = new object[args.Length + 1];
        Array.Copy(args, _args, args.Length);
        _startTimestamp = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        var delta = Stopwatch.GetElapsedTime(_startTimestamp);
        _args[^1] = delta.TotalMilliseconds;
        _logger.Log(_logLevel, 
            $"{_message} completed in {{OperationDurationMs}}ms", 
            _args);
    }
}
