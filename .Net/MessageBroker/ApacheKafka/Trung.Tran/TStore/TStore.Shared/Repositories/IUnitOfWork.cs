using System.Threading.Tasks;

namespace TStore.Shared.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<ITransaction> BeginTransactionAsync();
    }
}
