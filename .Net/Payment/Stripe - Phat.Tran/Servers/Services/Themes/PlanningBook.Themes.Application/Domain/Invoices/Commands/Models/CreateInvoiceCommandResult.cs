namespace PlanningBook.Themes.Application.Domain.Invoices.Commands.Models
{
    public class CreateInvoiceCommandResult
    {
        public Guid InvoiceId { get; set; }
        public string StripeSessionId { get; set; }
        public string? SubscriptionId { get; set; }
        public string Mode { get; set; }
        public string Url { get; set; }
        public string Currency { get; set; }
        public long? ActuallyAmountTotal { get; set; }
        public long? AmountTotal { get; set; }
    }
}
