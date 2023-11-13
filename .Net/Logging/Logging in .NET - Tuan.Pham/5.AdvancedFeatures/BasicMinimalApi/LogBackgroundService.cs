namespace BasicMinimalApi;

public class LogBackgroundService : BackgroundService
{
    private readonly ILogger<LogBackgroundService> _logger;

    public LogBackgroundService(ILogger<LogBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var paymentId = 1;
        var amount = 15.99;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "New Payment with id {PaymentId} for ${Total}", paymentId, amount);
            await Task.Delay(1000, stoppingToken);
        }
    }
}

