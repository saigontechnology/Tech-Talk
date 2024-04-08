namespace efcore_demos.Pieces;
internal class Interceptors : IExample
{
    private readonly DemoDbContext _dbContext;

    public Interceptors(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var products = await _dbContext.Products
            .Where(x => x.UpdatedDate == null)
            .Take(2)
            .ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            product.Quantity++;

            _dbContext.Products.Update(product);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
