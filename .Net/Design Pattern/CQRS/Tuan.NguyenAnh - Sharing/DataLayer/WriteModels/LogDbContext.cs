using Microsoft.EntityFrameworkCore;

namespace DataLayer.WriteModels
{
    public class LogDbContext : DbContext
    {
        public LogDbContext()
        {
        }

        public LogDbContext(string connectionString)
            : base(GetOptions(connectionString))
        {

        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public DbSet<SerializedAggregate> SerializedAggregate { get; set; }
        public DbSet<SerializedCommand> SerializedCommand { get; set; }
        public DbSet<SerializedEvent> SerializedEvent { get; set; }
        public DbSet<Snapshot> Snapshot { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("logs");

            builder.Entity<SerializedAggregate>(entity =>
            {
                entity.ToTable("Aggregate");
                entity.HasKey(x => x.AggregateIdentifier);

                entity.Property(x => x.AggregateClass).IsRequired().IsUnicode(false).HasMaxLength(200);
                entity.Property(x => x.AggregateExpires).IsRequired(false);
                entity.Property(x => x.AggregateIdentifier).IsRequired();
                entity.Property(x => x.AggregateType).IsRequired().IsUnicode(false).HasMaxLength(100);
            });

            builder.Entity<SerializedCommand>(entity =>
            {
                entity.ToTable("Command");
                entity.HasKey(x => x.CommandIdentifier);

                entity.Property(x => x.AggregateIdentifier).IsRequired();
                entity.Property(x => x.CommandClass).IsRequired().IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.CommandData).IsRequired().IsUnicode(true);
                entity.Property(x => x.CommandIdentifier).IsRequired();
                entity.Property(x => x.CommandType).IsRequired().IsUnicode(false).HasMaxLength(200);

                entity.Property(x => x.SendCompleted).IsRequired(false);
                entity.Property(x => x.SendStarted).IsRequired(false);
                entity.Property(x => x.SendStatus).IsRequired(false).IsUnicode(false).HasMaxLength(20);
            });

            builder.Entity<SerializedEvent>(entity =>
            {
                entity.ToTable("Event");
                entity.HasKey(x => new { x.AggregateIdentifier, x.AggregateVersion });

                entity.Property(x => x.EventClass).IsRequired().IsUnicode(false).HasMaxLength(200);
                entity.Property(x => x.EventType).IsRequired().IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.EventData).IsRequired().IsUnicode(true);
            });

            builder.Entity<Snapshot>(entity =>
            {
                entity.ToTable("Snapshot");
                entity.HasKey(x => x.AggregateIdentifier);

                entity.Property(x => x.AggregateIdentifier).IsRequired();
                entity.Property(x => x.AggregateState).IsRequired().IsUnicode(true);
                entity.Property(x => x.AggregateVersion).IsRequired();
            });

            base.OnModelCreating(builder);
        }
    }
}
