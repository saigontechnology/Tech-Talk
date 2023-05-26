using TStore.Shared.Entities;
using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, ITStoreUnitOfWork>
    {
    }

    public class OrderRepository : BaseRepository<Order, ITStoreUnitOfWork, TStoreContext>, IOrderRepository
    {
        public OrderRepository(TStoreContext dbContext, ITStoreUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
