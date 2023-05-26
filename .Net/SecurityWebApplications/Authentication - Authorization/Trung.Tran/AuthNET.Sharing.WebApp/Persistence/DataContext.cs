using Microsoft.EntityFrameworkCore;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext()
        {
        }

        public virtual DbSet<AppRole> Roles { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<AppUserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserPermissionGroup> UserPermissionGroups { get; set; }
        public virtual DbSet<AppUserRole> UserRoles { get; set; }
        public virtual DbSet<AppResource> Resources { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroups { get; set; }
        public virtual DbSet<PermissionGroupRecord> PermissionGroupRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(nameof(AuthNET));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(eBuilder =>
            {
                eBuilder.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AppRole>(eBuilder =>
            {
                eBuilder.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AppUserRole>(eBuilder =>
            {
                eBuilder.HasKey(e => e.Id);

                eBuilder.HasOne(e => e.User)
                    .WithMany(e => e.UserRoles)
                    .HasForeignKey(e => e.UserId);

                eBuilder.HasOne(e => e.Role)
                    .WithMany(e => e.UserRoles)
                    .HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<AppUserPermission>(eBuilder =>
            {
                eBuilder.HasKey(e => e.Id);

                eBuilder.HasOne(e => e.User)
                    .WithMany(e => e.UserPermissions)
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<AppResource>(eBuilder =>
            {
                eBuilder.HasKey(e => e.Id);

                eBuilder.HasOne(e => e.User)
                    .WithMany(e => e.Resources)
                    .HasForeignKey(e => e.UserId);
            });
        }
    }
}
