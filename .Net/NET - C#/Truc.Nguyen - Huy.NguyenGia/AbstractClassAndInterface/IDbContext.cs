using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{
    public interface IDbContext : IBaseDbContext
    {
       
    }

    public interface IBaseDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        EntityEntry Add(object entity);
    }
}
