namespace efcore_demos.Entities;
internal class InvoiceEntity : BaseEntity<int>
{
    public string InvoiceCode { get; set; }

    public string OrderCode { get; set; }

    public ContactEntity Contact { get; set; }

    public virtual ICollection<AddressEntity> Addresses { get; set; }
}
