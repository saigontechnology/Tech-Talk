using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.EF
{
    public interface IBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
         where TEntity : class
    {
    }
}
