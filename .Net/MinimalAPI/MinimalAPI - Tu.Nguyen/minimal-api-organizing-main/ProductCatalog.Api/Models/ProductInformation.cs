namespace ProductCatalog.Api.Models;

public class ProductInformation
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
}