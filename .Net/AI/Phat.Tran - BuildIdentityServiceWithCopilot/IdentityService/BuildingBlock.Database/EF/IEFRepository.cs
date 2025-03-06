using BuildingBlock.Database.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.EF
{
    public interface IEFRepository<TDbContext, TEntity, TPrimaryKey> : IRepository<TDbContext, TEntity, TPrimaryKey>
            where TDbContext : DbContext
            where TEntity : class
    {
    }
}
