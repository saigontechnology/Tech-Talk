namespace BasicMinimalApi;

public class TuanLoggerProvider : ILoggerProvider
{
    public void Dispose() { }

    public ILogger CreateLogger(string categoryName)
    {
        return new TuanLogger();
    }
}

public class TuanLogger : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel, EventId eventId, 
        TState state, Exception? exception, 
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        Console.WriteLine($"[{logLevel}({eventId})]: {state}");
    }
    
    internal sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();

        private NullScope()
        {
        }
        
        public void Dispose()
        {
        }
    }
}
