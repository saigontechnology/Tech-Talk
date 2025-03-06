using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.Core.Interfaces
{
    public interface IUnitOfWork<TDbContext> : IDisposable, IAsyncDisposable
        where TDbContext : DbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
