using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.Core.Interfaces
{
    public interface IRepository<TDbContext, TEntity, TPrimaryKey>
            where TDbContext : DbContext
            where TEntity : class
    {
        /// <summary>
        /// Directly to the Entities of this repository. Use for complex query
        /// </summary>
        /// <returns>DbSet<TEntity></returns>
        DbSet<TEntity> Entities();
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
