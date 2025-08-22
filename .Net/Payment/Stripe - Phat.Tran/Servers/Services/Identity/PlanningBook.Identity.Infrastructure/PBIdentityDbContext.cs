using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlanningBook.Identity.Infrastructure.Entities;
using System.Reflection;

namespace PlanningBook.Identity.Infrastructure
{
    public class PBIdentityDbContext : IdentityDbContext<Account, Role, Guid, AccountClaim, AccountRole, AccountLogin, RoleClaim, AccountToken>
    {
        public PBIdentityDbContext(DbContextOptions<PBIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
