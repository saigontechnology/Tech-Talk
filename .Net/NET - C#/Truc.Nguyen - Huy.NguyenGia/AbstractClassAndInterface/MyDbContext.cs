
namespace AbstractClassAndInterface
{
    public sealed partial class MyDbContext : BaseDBContext, IDbContext
    {

        protected override void OnConfiguring()
        {
            base.OnConfiguring();
            // do something
        }

        protected override void OnModelCreating()
        {
            base.OnModelCreating();
            // do something
        }
    }
}
