namespace SharedDomains;
public class StoreModel : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProductCategoryType Category { get; set; }
    public string Location { get; set; }

    public IEnumerable<ProductModel> Products { get; set; }

    public override void Generate()
    {
        Name = string.Join(" ", Faker.Lorem.Words(Faker.RandomNumber.Next(2, 5)));
        Description = Faker.Lorem.Sentence();
        Category = Faker.Enum.Random<ProductCategoryType>();
        Location = Faker.Address.Country();
    }
}
