using Microsoft.EntityFrameworkCore;

namespace WebAPI.Second;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<SharedDomains.ProductModel> Products { get; set; } = default!;
}
