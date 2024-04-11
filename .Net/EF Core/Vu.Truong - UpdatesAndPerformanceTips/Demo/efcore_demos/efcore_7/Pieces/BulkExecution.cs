namespace efcore_demos.Pieces;
internal class BulkExecution : IExample
{
    private readonly DemoDbContext _context;

    public BulkExecution(DemoDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _context.Products
            .Where(b => b.Quantity > 500)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.Orders
            .Where(b => b.Code.Contains("5"))
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Code, b => b.Code + " Five "), cancellationToken);

        await _context.SaveChangesAsync();
    }
}
