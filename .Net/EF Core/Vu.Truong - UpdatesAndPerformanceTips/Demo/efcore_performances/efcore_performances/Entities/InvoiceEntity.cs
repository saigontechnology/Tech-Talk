namespace efcore_performances.Entities;
internal class InvoiceEntity : BaseEntity<int>
{
    public string InvoiceCode { get; set; }

    public string OrderCode { get; set; }
}
