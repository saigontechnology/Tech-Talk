namespace efcore_demos.Pieces;
internal class JsonColumn : IExample
{
    private readonly DemoDbContext _dbContext;

    public JsonColumn(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.Users
            .Take(2)
            .Where(x => x.UpdatedDate == null)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            user.Address = new AddressEntity
            {
                City = Faker.Address.City(),
                Country = Faker.Address.Country(),
                Postcode = Faker.Address.UkPostCode(),
            };

            _dbContext.Update(user);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var orders = await _dbContext.Orders
            .Include(x => x.DeliveryDetail)
            .ThenInclude(x => x.Address)
            .Where(x => x.UpdatedDate == null)
            .Take(2)
            .ToListAsync(cancellationToken);

        foreach (var order in orders)
        {
            order.DeliveryDetail = new DeliveryDetailEntity
            {
                Note = Faker.Lorem.Sentence(),
                Tips = Faker.RandomNumber.Next(1000, 5000),
                Address = new AddressEntity
                {
                    City = Faker.Address.City(),
                    Country = Faker.Address.Country(),
                    Postcode = Faker.Address.UkPostCode(),
                }
            };

            _dbContext.Update(order);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var invoiceQuery = _dbContext.Invoices
            .Take(2)
            .Where(x => x.UpdatedDate == null);

        var invoices = invoiceQuery
            .ToList();

        foreach (var invoice in invoices)
        {
            var address = invoice.Addresses.FirstOrDefault();

            if (address is not null)
            {
                address.City = $"(updated) {address.City}";
            }

            invoice.UpdatedDate = DateTimeOffset.UtcNow;

            _dbContext.Update(invoice);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
