namespace efcore_demos.Pieces;
internal class CleanDatabase : IExample
{
    private readonly DemoDbContext _context;

    public CleanDatabase(DemoDbContext context)
    {
        _context = context;
    }

    const string RemoveAllQuery = $@"EXEC sp_clean_database";

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlRawAsync(RemoveAllQuery, cancellationToken);

        await _context.SaveChangesAsync();
    }
}
