using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TStore.Shared.Entities;

namespace TStore.Shared.Persistence
{
    public class InteractionContext : DbContext
    {
        public InteractionContext()
        {
            // [DEMO]
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        public InteractionContext(DbContextOptions<InteractionContext> options) : base(options)
        {
            // [DEMO]
            Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
        }

        public virtual DbSet<Interaction> Interactions { get; set; }
        public virtual DbSet<InteractionReport> InteractionReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost,1434;Database=KafkaTStoreInteraction;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // [DEMO] heavy task on db
            Thread.Sleep(new Random().Next(1000, 3000));
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // [DEMO] heavy task on db
            Thread.Sleep(new Random().Next(1000, 3000));
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
