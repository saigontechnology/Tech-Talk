using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TStore.Shared.Repositories
{
    public class BaseUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;

        public BaseUnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            return new Transaction(transaction);
        }
    }
}
