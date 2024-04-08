using Dumpify;
using efcore_demos.Constants;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace efcore_demos.DataAccess;
internal partial class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(b =>
        {
            b.ToTable(s => s.IsTemporal());
        });
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
    }
}

internal class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();

        Runner.Build((services, configuration) =>
        {
            string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
            connectionString.Dump("SQL Server Connection String");

            optionsBuilder.UseSqlServer(connectionString);
        });

        return new DemoDbContext(optionsBuilder.Options);
    }
}