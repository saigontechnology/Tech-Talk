using BasicLinQ.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicLinQ.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=BasicLinQ;Trusted_Connection=False;Encrypt=False;User Id=sa;Password=123456;MultipleActiveResultSets=true")
                    .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .IsRequired(false);

                builder.HasOne(e => e.Supplier)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired(false);

                builder.HasData(InitData.Products);
            });

            modelBuilder.Entity<ProductCategory>(builder =>
            {
                builder.HasOne(e => e.Supplier)
                    .WithMany(e => e.ProductCategories)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired(false);

                builder.HasData(InitData.Categories);
            });

            modelBuilder.Entity<Supplier>(builder =>
            {
                builder.HasData(InitData.Suppliers);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
