namespace efcore_demos.Entities;
internal class InvoiceEntity : BaseEntity<int>
{
    public string InvoiceCode { get; set; }
    public virtual ICollection<AddressEntity> Addresses { get; set; }
}
