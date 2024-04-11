namespace efcore_performances.Pieces;
internal class CleanDatabase : IExample
{
    private readonly DemoDbContext _context;

    public CleanDatabase(DemoDbContext context)
    {
        _context = context;
    }


    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
#if NET7_0_OR_GREATER
        await _context.Users.ExecuteDeleteAsync(cancellationToken);
        await _context.Products.ExecuteDeleteAsync(cancellationToken);
        await _context.Orders.ExecuteDeleteAsync(cancellationToken);
        await _context.Invoices.ExecuteDeleteAsync(cancellationToken);
#endif

        await _context.SaveChangesAsync(cancellationToken);
    }
}
