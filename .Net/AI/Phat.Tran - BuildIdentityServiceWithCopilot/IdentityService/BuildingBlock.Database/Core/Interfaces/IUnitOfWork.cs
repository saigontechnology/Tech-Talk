using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Database.Core.Interfaces
{
    public interface IUnitOfWork<TDbContext> : IDisposable, IAsyncDisposable
        where TDbContext : DbContext
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
