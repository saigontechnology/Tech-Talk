using efcore_demos.Constants;
using efcore_demos.DataAccess.Interceptors;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace efcore_demos.DataAccess;
internal partial class DemoDbContext : DbContext, IAuditDbContext
{
    private readonly SetRetrievedInterceptor _setRetrievedInterceptor;
    private readonly SetAuditInfoInterceptor _setAuditInfoInterceptor;

    public DemoDbContext(DbContextOptions<DemoDbContext> options,
        SetRetrievedInterceptor setRetrievedInterceptor,
        SetAuditInfoInterceptor setAuditInfoInterceptor
        ) : base(options)
    {
        _setRetrievedInterceptor = setRetrievedInterceptor;
        _setAuditInfoInterceptor = setAuditInfoInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(_setRetrievedInterceptor);
        optionsBuilder.AddInterceptors(_setAuditInfoInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().OwnsOne(
            user => user.Address, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });

        modelBuilder.Entity<OrderEntity>().OwnsOne(
            order => order.DeliveryDetail, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsOne(deliveryDetails => deliveryDetails.Address);
            });

        modelBuilder.Entity<InvoiceEntity>().OwnsMany(
            invoice => invoice.Addresses, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property<string>("_identifier");
            });

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

        return new DemoDbContext(optionsBuilder.Options, null, null);
    }
}