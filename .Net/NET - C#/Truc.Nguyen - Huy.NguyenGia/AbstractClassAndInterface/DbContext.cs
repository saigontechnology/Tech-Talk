using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace AbstractClassAndInterface
{
    public partial class MyDbContext
    {
        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(1);
        }

        public EntityEntry Add(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
