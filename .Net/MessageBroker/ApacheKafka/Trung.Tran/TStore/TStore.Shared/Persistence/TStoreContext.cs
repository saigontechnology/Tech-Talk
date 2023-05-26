using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TStore.Shared.Entities;

namespace TStore.Shared.Persistence
{
    public class TStoreContext : DbContext
    {
        public TStoreContext()
        {
            // [DEMO]
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        public TStoreContext(DbContextOptions<TStoreContext> options) : base(options)
        {
            // [DEMO]
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // [DEMO] heavy task on db
            Thread.Sleep(new Random().Next(500, 2000));
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // [DEMO] heavy task on db
            Thread.Sleep(new Random().Next(500, 2000));
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer("Server=localhost,1434;Database=KafkaTStore;Trusted_Connection=False;User Id=sa;Password=z@123456!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasMany(e => e.OrderItems)
                    .WithOne(e => e.Order)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(e => e.OrderItems)
                    .WithOne(e => e.Product)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasData(GetInitProducts());
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });
            });
        }

        public Product[] GetInitProducts() => new[]
        {
            new Product { Id = Guid.NewGuid(), Name = "Laptop", Price = 1000 },
            new Product { Id = Guid.NewGuid(), Name = "Television", Price = 2000 },
            new Product { Id = Guid.NewGuid(), Name = "Telephone", Price = 300 },
            new Product { Id = Guid.NewGuid(), Name = "Iphone 14", Price = 5000 },
            new Product { Id = Guid.NewGuid(), Name = "Samsung Galaxy", Price = 2000 },
            new Product { Id = Guid.NewGuid(), Name = "Xbox", Price = 7000 },
        };
    }
}
