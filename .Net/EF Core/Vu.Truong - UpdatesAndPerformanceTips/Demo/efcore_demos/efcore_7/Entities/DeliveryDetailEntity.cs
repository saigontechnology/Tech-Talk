namespace efcore_demos.Entities;
internal record DeliveryDetailEntity
{
    public string Note { get; set; }

    public decimal Tips { get; set; }

    public AddressEntity Address { get; set; }
}
