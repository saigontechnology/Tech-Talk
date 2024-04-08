namespace efcore_demos.Entities;
internal class OrderEntity : BaseRetrievedEntity
{
    public Guid ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }

    public string Code { get; set; }

    public virtual DeliveryDetailEntity DeliveryDetail { get; set; }
}
