using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedisSharing.UseCases.ShoppingCart.Models;

namespace RedisSharing.UseCases.ShoppingCart.SqlServerStore
{
    public class ShoppingCartContext : DbContext, IDesignTimeDbContextFactory<ShoppingCartContext>
    {
        public ShoppingCartContext(DbContextOptions options) : base(options)
        {
        }

        public ShoppingCartContext()
        {
        }

        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=SqlServerShoppingCart;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCartItem>(e =>
            {
                e.HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductName);
                e.HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserName);
            });

            modelBuilder.Entity<AppUser>(e => e.HasKey(o => o.Name));
            modelBuilder.Entity<Product>(e => e.HasKey(o => o.Name));
        }

        public ShoppingCartContext CreateDbContext(string[] args)
        {
            return new ShoppingCartContext();
        }
    }
}
