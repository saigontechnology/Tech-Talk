namespace SharedDomains;
public class ProductModel : BaseModel
{
    public string Name { get; set; } 
    public string Description { get; set; }
    public long Quantity { get; set; }
    public long Price { get; set; }

    public ProductCategoryType Category { get; set; }

    public override void Generate()
    {
        Name = string.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(2, 5)));
        Description = Faker.Lorem.Sentence();
        Quantity = Faker.RandomNumber.Next(10, 100);
        Price = Faker.RandomNumber.Next(10, 20) * 100;
        Category = Faker.Enum.Random<ProductCategoryType>();
    }
}
