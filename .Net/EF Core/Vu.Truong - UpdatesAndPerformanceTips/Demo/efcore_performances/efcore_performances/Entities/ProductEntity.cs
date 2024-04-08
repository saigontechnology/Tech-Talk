namespace efcore_performances.Entities;
internal class ProductEntity : BaseRetrievedEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public Money Price { get; set; }
}