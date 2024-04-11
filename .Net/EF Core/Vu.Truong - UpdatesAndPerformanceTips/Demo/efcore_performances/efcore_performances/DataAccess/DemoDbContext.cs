using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace efcore_performances.DataAccess;
internal partial class DemoDbContext : DbContext, IAuditDbContext
{
    public static long InstanceCount;
    public static long DisposedCount;

    private bool _disposed = false;

    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
        Interlocked.Increment(ref InstanceCount);
    }

    public override void Dispose()
    {
        if (!_disposed)
        {
            Interlocked.Increment(ref DisposedCount);
            _disposed = true;
        }

        base.Dispose();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<string>()
            .AreUnicode(false)
            .HaveMaxLength(1024);

        configurationBuilder
            .Properties<bool>()
            .HaveConversion<BoolToZeroOneConverter<int>>();

        configurationBuilder
            .Properties<Money>()
            .HaveConversion<MoneyConverter>()
            .HaveMaxLength(64);

        configurationBuilder
            .Properties<List<DateOnly>>()
            .AreUnicode(false)
            .HaveMaxLength(4000);
    }
}

internal class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        Container.GetInstance("Migrations").Build((services, configuration) =>
        {
            string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
            connectionString.Dump("SQL Server Connection String");

            optionsBuilder.UseSqlServer(connectionString);
        });

        return new DemoDbContext(optionsBuilder.Options);
    }
}