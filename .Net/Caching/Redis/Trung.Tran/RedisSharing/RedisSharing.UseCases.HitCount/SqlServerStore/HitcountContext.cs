using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedisSharing.UseCases.HitCount.Models;

namespace RedisSharing.UseCases.HitCount.SqlServerStore
{
    public class HitcountContext : DbContext, IDesignTimeDbContextFactory<HitcountContext>
    {
        public HitcountContext(DbContextOptions options) : base(options)
        {
        }

        public HitcountContext()
        {
        }

        public virtual DbSet<HitCountItem> HitCountItems { get; set; }
        public virtual DbSet<IpAddress> IpAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=SqlServerHitCount;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HitCountItem>(e => e.HasKey(o => o.Key));
            modelBuilder.Entity<IpAddress>(e => e.HasKey(o => o.Ip));
        }

        public HitcountContext CreateDbContext(string[] args)
        {
            return new HitcountContext();
        }
    }
}
