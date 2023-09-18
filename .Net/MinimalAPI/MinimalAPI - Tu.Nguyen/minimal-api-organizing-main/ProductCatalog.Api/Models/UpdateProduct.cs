namespace ProductCatalog.Api.Models;

public class UpdateProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
}