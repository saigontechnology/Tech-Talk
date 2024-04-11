namespace efcore_demos.DataAccess;
internal partial class DemoDbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ProductEntity> Products { get; set; }
}
