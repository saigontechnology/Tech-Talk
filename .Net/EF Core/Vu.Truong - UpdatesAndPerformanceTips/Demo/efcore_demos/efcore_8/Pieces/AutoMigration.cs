namespace efcore_demos.Pieces;
internal class AutoMigration : IExample
{
    private readonly DemoDbContext _dbContext;

    public AutoMigration(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task IExample.ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }
}
