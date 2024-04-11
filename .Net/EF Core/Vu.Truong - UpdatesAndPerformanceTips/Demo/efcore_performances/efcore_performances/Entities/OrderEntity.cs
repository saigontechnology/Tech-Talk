namespace efcore_performances.Entities;
internal class OrderEntity : BaseRetrievedEntity
{
    public Guid ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }

    public string Code { get; set; }
}
