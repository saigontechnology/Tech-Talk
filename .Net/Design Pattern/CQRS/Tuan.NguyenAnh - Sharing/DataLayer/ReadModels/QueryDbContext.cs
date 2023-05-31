using Microsoft.EntityFrameworkCore;

namespace DataLayer.Query
{
    public class QueryDbContext : DbContext
    {
        public QueryDbContext()
        {
        }

        public QueryDbContext(string connectionString)
            : base(GetOptions(connectionString))
        {
            
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public DbSet<AccountSummary> AccountSummaries { get; set; }
        public DbSet<UserSummary> UserSummaries { get; set; }
        public DbSet<TransferSummary> TransferSummaries { get; set; }

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
            builder.HasDefaultSchema("queries");

            builder.Entity<UserSummary>(entity =>
            {
                entity.ToTable("UserSummary");
                entity.HasKey(x => x.UserIdentifier);

                entity.Property(x => x.OpenAccountCount).IsRequired();
                entity.Property(x => x.TotalAccountBalance).IsRequired();
                entity.Property(x => x.UserIdentifier).IsRequired();
                entity.Property(x => x.Name).IsRequired(false).IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.UserRegistered).IsRequired();

                entity.Property(x => x.LoginName).IsRequired().IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.LoginPassword).IsRequired().IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.UserRegistrationStatus).IsRequired().IsUnicode(false).HasMaxLength(10);
            });

            builder.Entity<AccountSummary>(entity =>
            {
                entity.ToTable("AccountSummary");
                entity.HasKey(x => x.AccountIdentifier);

                entity.Property(x => x.AccountCode).IsRequired(false).IsUnicode(false).HasMaxLength(100);
                entity.Property(x => x.AccountIdentifier).IsRequired();
                entity.Property(x => x.AccountBalance).IsRequired();
                entity.Property(x => x.OwnerIdentifier).IsRequired();
            });

            builder.Entity<TransferSummary>(entity =>
            {
                entity.ToTable("TransferSummary");
                entity.HasKey(x => x.TransferIdentifier);

                entity.Property(x => x.FromAccountIdentifier).IsRequired();
                entity.Property(x => x.ToAccountIdentifier).IsRequired();
                entity.Property(x => x.TransferAmount).IsRequired();
                entity.Property(x => x.TransferIdentifier).IsRequired();
                entity.Property(x => x.TransferStatus).IsRequired(false).IsUnicode(false).HasMaxLength(10);
            });

            base.OnModelCreating(builder);
        }
    }
}
