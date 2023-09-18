namespace ProductCatalog.Api.Data.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; }
}