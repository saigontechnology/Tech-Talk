using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlanningBook.DBEngine
{
    public interface IBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
         where TEntity : class
    {
        //void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
