using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedisSharing.UseCases.ScoringTable.Models;

namespace RedisSharing.UseCases.ScoringTable.SqlServerStore
{
    public class ScoringContext : DbContext, IDesignTimeDbContextFactory<ScoringContext>
    {
        public ScoringContext(DbContextOptions options) : base(options)
        {
        }

        public ScoringContext()
        {
        }

        public virtual DbSet<PlayerRankRecord> PlayerRankRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=SqlServerScoringTable;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlayerRankRecord>(e =>
            {
                e.HasKey(o => o.Id).IsClustered(false);
                e.HasIndex(o => o.Score).IsClustered();
                e.HasIndex(o => o.RecordedTime).IsClustered(false);
            });
        }

        public ScoringContext CreateDbContext(string[] args)
        {
            return new ScoringContext();
        }
    }
}
