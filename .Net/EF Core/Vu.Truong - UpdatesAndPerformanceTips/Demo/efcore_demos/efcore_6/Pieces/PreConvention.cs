using Dumpify;

namespace efcore_demos.Pieces;
internal class PreConvention : IExample
{
    private readonly DemoDbContext _dbContext;

    const int Total = 10;

    public PreConvention(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Enumerable.Range(0, Total)
            .Select(x =>
            {
                var product = new ProductEntity
                {
                    Name = $"Product_{x + 1}",
                    Description = $"Product {x + 1}",
                    Price = new Money(x * 100, x % 2 == 0 ? Currency.UsDollars : Currency.PoundsSterling),
                    Quantity = 100
                };

                _dbContext.Add(product);

                return 0;
            })
            .ToList();

        await _dbContext.SaveChangesAsync(cancellationToken);

        var products = await _dbContext.Products.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        foreach(var product in products)
        {
            product.Dump();
            Console.WriteLine("Price: " + product.Price.ToString());
        }
    }
}
