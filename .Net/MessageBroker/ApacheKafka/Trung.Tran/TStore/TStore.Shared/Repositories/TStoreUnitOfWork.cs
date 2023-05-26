using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface ITStoreUnitOfWork : IUnitOfWork
    {
    }

    public class TStoreUnitOfWork : BaseUnitOfWork<TStoreContext>, ITStoreUnitOfWork
    {
        public TStoreUnitOfWork(TStoreContext dbContext) : base(dbContext)
        {
        }
    }
}
