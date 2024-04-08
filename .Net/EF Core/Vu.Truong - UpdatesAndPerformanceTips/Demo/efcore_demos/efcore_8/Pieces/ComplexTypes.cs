
namespace efcore_demos.Pieces;
internal class ComplexTypes : IExample
{
    private readonly DemoDbContext _dbContext;

    public ComplexTypes(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {

        var product = await _dbContext.Products.FirstOrDefaultAsync(cancellationToken);
        var id = await _dbContext.Orders.Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        var address = await _dbContext.Orders
            .Select(x => x.BillingAddress)
            .FirstOrDefaultAsync(cancellationToken);

        var order = new OrderEntity
        {
            BillingAddress = address,
            ShippingAddress = address,
            Code = "Dump Code",
            ProductId = product.Id
        };

        var order2 = new OrderEntity
        {
            BillingAddress = address,
            ShippingAddress = address,
            Code = "Dump Code 2",
            ProductId = product.Id
        };

        _dbContext.Orders.Add(order);
        _dbContext.Orders.Add(order2);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // This reference will work on memory, because the secret key is stored on memory
        order.BillingAddress.City = $"Changed-{DateTime.UtcNow.ToFileTimeUtc()}-{Faker.Address.City()}";
        await _dbContext.SaveChangesAsync(cancellationToken);

        var phoneNumber = new PhoneNumberEntity(62, 848695568);

        var contact = await _dbContext.Invoices
            .Select(x => x.Contact)
            .Where(x => x.PhoneNumber == phoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

        contact.Dump();
    }
}
