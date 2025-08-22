using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace PlanningBook.Themes.Infrastructure
{
    public class PBThemeDbContext : DbContext
    {
        public PBThemeDbContext(DbContextOptions<PBThemeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
