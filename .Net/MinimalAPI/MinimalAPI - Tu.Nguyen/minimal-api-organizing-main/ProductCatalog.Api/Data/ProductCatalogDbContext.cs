using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data.Entities;

namespace ProductCatalog.Api.Data;

public class ProductCatalogDbContext : DbContext
{
    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCatalogDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    #region Tables

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    #endregion

    #region Views

    //public DbSet<ModelView> Views { get; set; }

    #endregion

    #region DbObjects - Stored

    //public DbSet<ModelStoredProcedure> StoredProcedure { get; set; }

    #endregion
}