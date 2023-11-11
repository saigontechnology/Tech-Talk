using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace LoggingBestPractices.Benchmarks;

public class FakeLoggingProvider : ILoggerProvider
{
    public void Dispose()
    {

    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FakeLogger();
    }
}

public class FakeLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new FakeDisposable();
    }

    private class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

public static class FakeLoggerExtensions
{
    public static ILoggingBuilder AddFakeLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FakeLoggingProvider>());
        LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, FakeLoggingProvider>(builder.Services);
        return builder;
    }
}
