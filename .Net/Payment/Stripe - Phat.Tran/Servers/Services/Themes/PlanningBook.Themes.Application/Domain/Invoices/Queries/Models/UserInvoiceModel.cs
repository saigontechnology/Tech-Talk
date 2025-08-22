namespace PlanningBook.Themes.Application.Domain.Invoices.Queries.Models
{
    public sealed class UserInvoiceModel
    {
        public Guid InvoiceId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
