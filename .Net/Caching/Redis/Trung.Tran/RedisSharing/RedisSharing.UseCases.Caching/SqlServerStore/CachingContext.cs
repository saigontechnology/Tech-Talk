using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedisSharing.UseCases.Caching.Models;

namespace RedisSharing.UseCases.Caching.SqlServerStore
{
    public class CachingContext : DbContext, IDesignTimeDbContextFactory<CachingContext>
    {
        public CachingContext(DbContextOptions options) : base(options)
        {
        }

        public CachingContext()
        {
        }

        public virtual DbSet<CountryItem> Countries { get; set; }
        public virtual DbSet<CountryState> States { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=SqlServerCaching;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CountryItem>(e =>
            {
                e.HasKey(o => o.Name);
                e.Property(o => o.Name).HasMaxLength(256);
            });
            modelBuilder.Entity<CountryState>(e =>
            {
                e.Property(o => o.Name).HasMaxLength(256);
                e.HasKey(o => new
                {
                    o.Name,
                    o.CountryName
                });

                e.HasOne(o => o.Country)
                    .WithMany(o => o.States)
                    .HasForeignKey(o => o.CountryName);
            });
        }

        public CachingContext CreateDbContext(string[] args)
        {
            return new CachingContext();
        }
    }
}
