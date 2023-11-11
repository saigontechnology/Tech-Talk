using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace LoggingBestPractices.Benchmarks;

[MemoryDiagnoser]
public class Benchmarkie
{
    private const string LogMessageWithParameters =
        "This is a log message with parameters {First}, {Second}";

    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddFakeLogger().SetMinimumLevel(LogLevel.Warning);
    });

    private readonly ILogger<Benchmarkie> _logger;
    private readonly ILoggerAdapter<Benchmarkie> _loggerAdapter;

    public Benchmarkie()
    {
        _logger = new Logger<Benchmarkie>(_loggerFactory);
        _loggerAdapter = new LoggerAdapter<Benchmarkie>(_logger);
    }

    [Benchmark]
    public void Log_WithoutIf_WithParams()
    {
        _logger.LogInformation(LogMessageWithParameters, 69, 420);
    }

    [Benchmark]
    public void Log_WithIf_WithParams()
    {
        if(_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(LogMessageWithParameters, 69, 420);
        }
    }

    [Benchmark]
    public void LogAdapter_WithoutIf_WithParams()
    {
        _loggerAdapter.LogInformation(LogMessageWithParameters, 69, 420);
    }

    [Benchmark]
    public void LoggerMessageDef_WithoutIf_Warning()
    {
        _logger.LogBenchmarkMessage(69, 420);
    }

    [Benchmark]
    public void LoggerMessage_SourceGen_Warning()
    {
        _logger.LogBenchmarkMessageGen(69, 420);
    }
}
