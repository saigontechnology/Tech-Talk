using Framework.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.EF
{
    public class EFRepository<TDbContext, TEntity, TPrimaryKey>(TDbContext dbContext) : IEFRepository<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly TDbContext _dbContext = dbContext;

        public DbSet<TEntity> Entities()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ProcessDateAudited(entity, false);
            await Entities().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                ProcessDateAudited(entity, false);
            await Entities().AddRangeAsync(entities, cancellationToken);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Entities().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Entities().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ProcessDateAudited(entity, true);
            Entities().Update(entity);
            return Task.CompletedTask;

        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                ProcessDateAudited(entity, true);
            Entities().UpdateRange(entities);
            return Task.CompletedTask;
        }

        #region Private Methods
        private void ProcessDateAudited(TEntity entity, bool isUpdate)
        {
            bool isDateAudited = entity is IDateAudited;
            if (!isDateAudited)
                return;

            if (isUpdate)
            {
                (entity as IDateAudited).UpdatedDate = DateTime.Now;
            }
            else
            {
                (entity as IDateAudited).CreatedDate = DateTime.Now;
                (entity as IDateAudited).UpdatedDate = DateTime.Now;
            }
        }
        #endregion
    }
}
