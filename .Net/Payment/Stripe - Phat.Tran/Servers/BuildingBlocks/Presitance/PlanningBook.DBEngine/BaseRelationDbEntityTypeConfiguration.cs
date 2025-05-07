using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace PlanningBook.DBEngine
{
    public abstract class BaseRelationDbEntityTypeConfiguration<TEntity> : IBaseEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable($"{typeof(TEntity).Name}s");

            var hasIdColumn = typeof(TEntity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if(hasIdColumn != null)
            {
                builder.HasKey("Id");
            }
        }
    }
}