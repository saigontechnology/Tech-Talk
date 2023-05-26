using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TAuth.ResourceAPI.Services;

namespace TAuth.ResourceAPI.Entities
{
    public class ResourceContext : DbContext
    {
        public const string DefaultConnStr = "Data Source=./ResourceContext.db";

        private IUserProvider _userProvider;

        public ResourceContext(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public ResourceContext(DbContextOptions options, IUserProvider userProvider) : base(options)
        {
            _userProvider = userProvider;
        }

        public virtual DbSet<ResourceEntity> Resources { get; set; }
        public virtual DbSet<ApplicationUserClaim> UserClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(DefaultConnStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResourceEntity>(eBuilder =>
            {
                eBuilder.HasQueryFilter(o => o.OwnerId == _userProvider.CurrentUserId);
            });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntitiesOwner();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetEntitiesOwner();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected void SetEntitiesOwner()
        {
            if (_userProvider.CurrentUserId == null) return;

            var ownedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IOwnedEntity>().ToArray();

            foreach (var entry in ownedEntities)
            {
                entry.OwnerId = _userProvider.CurrentUserId;
            }
        }
    }
}
