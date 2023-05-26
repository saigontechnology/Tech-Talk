using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace TAuth.IDP.Models
{
    // Use ConfigurationDbContext, PersistedGrantDbContext for customization (without IdentityFramework)
    public class IdpContext : ApiAuthorizationDbContext<AppUser>, IConfigurationDbContext, IPersistedGrantDbContext
    {
        public IdpContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=./TAuthIDP.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSecret>(eBuilder =>
            {
                eBuilder.HasOne(e => e.User)
                    .WithMany(e => e.UserSecrets)
                    .HasForeignKey(e => e.UserId);
            });
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<UserSecret> UserSecrets { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(default);
        }
    }
}
