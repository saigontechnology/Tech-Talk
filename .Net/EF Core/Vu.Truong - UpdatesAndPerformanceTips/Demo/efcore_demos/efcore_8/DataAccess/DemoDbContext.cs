using efcore_demos.Constants;
using efcore_demos.DataAccess.Interceptors;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace efcore_demos.DataAccess;
internal partial class DemoDbContext : DbContext, IAuditDbContext
{
    private readonly SetAuditInfoInterceptor _setAuditInfoInterceptor;

    public DemoDbContext(DbContextOptions<DemoDbContext> options, SetAuditInfoInterceptor setAuditInfoInterceptor) : base(options)
    {
        _setAuditInfoInterceptor = setAuditInfoInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_setAuditInfoInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().OwnsOne(
            user => user.Address, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });

        modelBuilder.Entity<UserEntity>(
            b =>
            {
                b.Property(e => e.DaysVisited)
                    .HasMaxLength(4028)
                    .IsUnicode(false);

                b.Property(e => e.OrderHistories)
                    .HasMaxLength(4028)
                    .IsUnicode(false);
            }
        );

        modelBuilder.Entity<OrderEntity>(
            b =>
            {
                b.OwnsOne(
                    order => order.DeliveryDetail, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.ToJson();
                        ownedNavigationBuilder.OwnsOne(deliveryDetails => deliveryDetails.Address);
                    }
                );

                b.ComplexProperty(e => e.BillingAddress)
                    .IsRequired();
                b.ComplexProperty(e => e.ShippingAddress)
                    .IsRequired();
            }
        );

        modelBuilder.Entity<InvoiceEntity>(
            b =>
            {
                b.OwnsMany(
                    invoice => invoice.Addresses, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.ToJson();
                    });

                b.ComplexProperty(e => e.Contact,
                    c =>
                    {
                        c.IsRequired();
                        c.ComplexProperty(e => e.Address).IsRequired();
                        c.ComplexProperty(e => e.PhoneNumber).IsRequired();
                    });
            }
        );

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

        Runner.Build((services, configuration) =>
        {
            string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
            connectionString.Dump("SQL Server Connection String");

            optionsBuilder.UseSqlServer(connectionString, x => x.UseHierarchyId());
        });

        return new DemoDbContext(optionsBuilder.Options, null);
    }
}