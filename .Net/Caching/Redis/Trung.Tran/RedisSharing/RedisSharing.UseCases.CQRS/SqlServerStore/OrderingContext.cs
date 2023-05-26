using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedisSharing.UseCases.CQRS.Models;

namespace RedisSharing.UseCases.CQRS.SqlServerStore
{
    public class OrderingContext : DbContext, IDesignTimeDbContextFactory<OrderingContext>
    {
        public OrderingContext(DbContextOptions options) : base(options)
        {
        }

        public OrderingContext()
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=SqlServerOrdering;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductId);
                e.HasOne(o => o.Order).WithMany(o => o.Items).HasForeignKey(o => o.OrderId);
            });
        }

        public OrderingContext CreateDbContext(string[] args)
        {
            return new OrderingContext();
        }
    }
}
