using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace BasicConsoleApp;

public class TuanSink : ILogEventSink
{
    private readonly IFormatProvider? _formatProvider;

    public TuanSink(IFormatProvider? formatProvider)
    {
        _formatProvider = formatProvider;
    }
    
    public TuanSink() : this(null)
    {
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Console.WriteLine($"{DateTime.UtcNow} - {message}");
    }
}


public static class TuanSinkExtensions
{
    public static LoggerConfiguration TuanSink(
        this LoggerSinkConfiguration sinkConfiguration,
        IFormatProvider? formatProvider = null)
    {
        return sinkConfiguration.Sink(new TuanSink());
    }
}
