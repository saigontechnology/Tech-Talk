using TStore.Shared.Entities;
using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface IProductRepository : IBaseRepository<Product, ITStoreUnitOfWork>
    {
    }

    public class ProductRepository : BaseRepository<Product, ITStoreUnitOfWork, TStoreContext>, IProductRepository
    {
        public ProductRepository(TStoreContext dbContext, ITStoreUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
