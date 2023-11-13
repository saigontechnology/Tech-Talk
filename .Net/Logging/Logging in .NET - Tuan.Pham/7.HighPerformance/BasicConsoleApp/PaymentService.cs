using Microsoft.Extensions.Logging;

namespace BasicConsoleApp;

public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;

    // private static readonly Action<ILogger, string, decimal, int, Exception?> _logPayment =
    //     LoggerMessage.Define<string, decimal, int>(
    //         LogLevel.Information,
    //         new EventId(5001, nameof(CreatePayment)),
    //         "Customer {Email} purchased product {ProductId} at {Amount}"
    //     );

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }

    public void CreatePayment(string email, decimal amount, int productId)
    {
        // Do some work
        _logger.LogPaymentCreation(email, amount, productId);
    }
}
// _logPayment(_logger, email, amount, productId, null);

