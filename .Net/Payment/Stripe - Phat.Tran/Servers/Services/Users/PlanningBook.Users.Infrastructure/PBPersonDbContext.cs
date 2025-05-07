using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace PlanningBook.Users.Infrastructure
{
    public class PBPersonDbContext : DbContext
    {
        public PBPersonDbContext(DbContextOptions<PBPersonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
