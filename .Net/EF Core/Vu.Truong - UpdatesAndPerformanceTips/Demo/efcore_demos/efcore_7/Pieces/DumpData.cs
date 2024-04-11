using System.Linq;

namespace efcore_demos.Pieces;
internal class DumpData : IExample
{
    private readonly DemoDbContext _dbContext;

    const int Rows = 100;

    public DumpData(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Dump data
        var products = Enumerable.Range(0, Rows)
                .Select(i => new ProductEntity
                {
                    Description = Faker.Company.Name(),
                    Name = Faker.Company.Name(),
                    Price = new Money(Faker.RandomNumber.Next(100, 1000), Faker.Enum.Random<Currency>()),
                    Quantity = Faker.RandomNumber.Next(50, 100)
                }).ToList();

        _dbContext.Products.AddRange(products);

        var orders = products
            .Select((p, i) => new OrderEntity
            {
                Code = $"Code {i}",
                ProductId = p.Id,
                DeliveryDetail = EntityHelper.GenerateDeliveryDetail()
            })
            .ToList();

        _dbContext.Orders.AddRange(orders);

        _dbContext.Users.AddRange(
            Enumerable.Range(0, 100)
                .Select(i => new UserEntity
                {
                    Email = Faker.Internet.Email(),
                    BirthDate = Faker.Identification.DateOfBirth(),
                    Password = Faker.Identification.UsPassportNumber(),
                    UserName = Faker.Internet.UserName(),
                    Address = EntityHelper.GenerateAddress()
                })
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        _dbContext.Invoices.AddRange(
            Enumerable.Range(0, 100)
                .Select(i => new InvoiceEntity
                {
                    InvoiceCode = $"Invoie Code {i}",
                    Addresses = new List<AddressEntity>
                    {
                        EntityHelper.GenerateAddress()
                    }
                })
        );

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
