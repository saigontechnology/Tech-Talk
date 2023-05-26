using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TStore.Shared.Repositories
{
    public interface IBaseRepository<T, TUoW>
        where T : class
        where TUoW : IUnitOfWork
    {
        IQueryable<T> Get();
        T Create(T entity);
        void Create(IEnumerable<T> entities);
        T Update(T entity);
        T Attach(T entity);
        T Delete(T entity);
        Task<int> ExecuteAsync(FormattableString cmd);
        TUoW UnitOfWork { get; }
    }

    public class BaseRepository<T, TUoW, TDbContext> : IBaseRepository<T, TUoW>
        where T : class
        where TUoW : IUnitOfWork
        where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        private readonly TUoW _unitOfWork;
        public TUoW UnitOfWork => _unitOfWork;

        public BaseRepository(TDbContext dbContext, TUoW unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public T Create(T entity)
        {
            return _dbContext.Add(entity).Entity;
        }

        public void Create(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public T Delete(T entity)
        {
            return _dbContext.Remove(entity).Entity;
        }

        public IQueryable<T> Get()
        {
            return DbSet;
        }

        public T Update(T entity)
        {
            return _dbContext.Update(entity).Entity;
        }

        public T Attach(T entity)
        {
            return _dbContext.Attach(entity).Entity;
        }

        public Task<int> ExecuteAsync(FormattableString cmd)
        {
            return _dbContext.Database.ExecuteSqlInterpolatedAsync(cmd);
        }
    }
}
