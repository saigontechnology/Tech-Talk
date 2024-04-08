using Dumpify;
using efcore_demos.Constants;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace efcore_demos.DataAccess;
internal class DemoCompiledDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ProductEntity> Products { get; set; }

    public DemoCompiledDbContext(DbContextOptions<DemoCompiledDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseModel(efcore_6.DemoCompiledModel.DemoCompiledDbContextModel.Instance);
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

internal class DemoCompiledDbContextFactory : IDesignTimeDbContextFactory<DemoCompiledDbContext>
{
    public DemoCompiledDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoCompiledDbContext>();

        Runner.Build((services, configuration) =>
        {
            string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString_Second);
            connectionString.Dump("SQL Server Connection String");

            optionsBuilder.UseSqlServer(connectionString);
        });

        return new DemoCompiledDbContext(optionsBuilder.Options);
    }
}