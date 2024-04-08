namespace efcore_demos.Pieces;
internal class PrimitiveCollections : IExample
{
    private readonly DemoDbContext _dbContext;

    public PrimitiveCollections(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var user = await  _dbContext.Users.FirstOrDefaultAsync(cancellationToken);
        user.Dump();
        user.DaysVisited ??= new List<DateOnly>();

        user.DaysVisited.Add(DateOnly.FromDateTime(DateTime.UtcNow));

        await _dbContext.SaveChangesAsync(cancellationToken);

        var orders = await _dbContext.Orders
            .Select(x => x.Id)
            .Take(5)
            .ToListAsync(cancellationToken);

        user.OrderHistories ??= new List<Guid>();
        user.OrderHistories.AddRange(orders);

        await _dbContext.SaveChangesAsync(cancellationToken);
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var toDate = date.AddDays(10);
        var user2 = await _dbContext.Users
            .Where(x => x.DaysVisited.Any(d => d >= date && d <= toDate))
            .FirstOrDefaultAsync(cancellationToken);

        user2.Dump();
    }
}