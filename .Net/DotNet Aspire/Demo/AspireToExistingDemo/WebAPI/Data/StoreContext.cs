using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<SharedDomains.StoreModel> StoreModel { get; set; } = default!;
    }
}
