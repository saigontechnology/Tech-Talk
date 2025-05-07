using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlanningBook.Domain;
using PlanningBook.Domain.Constant;
using PlanningBook.Domain.Enums;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace PlanningBook.Repository.EF
{
    public class EFRepository<TDbContext, TEntity, TPrimaryKey> : IEFRepository<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
    {
        protected readonly TDbContext _dbContext;
        public EFRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ISoftDeleted)
            {
                ((ISoftDeleted)entity).IsDeleted = false;
                ((ISoftDeleted)entity).DeletedAt = null;
            }

            if (entity is IActiveEntity)
            {
                ((IActiveEntity)entity).IsActive = true;
            }

            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.FirstOrDefault() is IDateAudited)
            {
                foreach (var entity in entities)
                {
                    if (entity is IDateAudited)
                    {
                        ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                        ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
                    }

                    if (entity is ISoftDeleted)
                    {
                        ((ISoftDeleted)entity).IsDeleted = false;
                        ((ISoftDeleted)entity).DeletedAt = null;
                    }

                    if (entity is IActiveEntity)
                    {
                        ((IActiveEntity)entity).IsActive = true;
                    }
                }
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            if (whereCondition == null)
                return await _dbContext.Set<TEntity>().CountAsync(cancellationToken);

            return await _dbContext.Set<TEntity>().CountAsync(whereCondition, cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereCondition, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
            var elementType = result.ElementType;
            var isDateAuditable = typeof(IDateAudited).IsAssignableFrom(elementType);

            if (whereCondition != null)
                result = result.Where(whereCondition);

            if (sortCriteria.Any())
            {
                bool isFirstSortQuery = true;
                foreach (var criteria in sortCriteria)
                {
                    var sortColumnName = criteria.Item1;
                    var sortDirection = criteria.Item2;

                    if (isFirstSortQuery)
                    {
                        isFirstSortQuery = false;
                        result = sortDirection == SortDirection.Ascending ?
                            result.OrderBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName)) :
                            result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                    else
                    {
                        result = sortDirection == SortDirection.Ascending
                            ? ((IOrderedQueryable<TEntity>)result).ThenBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName))
                            : ((IOrderedQueryable<TEntity>)result).ThenByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                }
            }
            else
            {
                PropertyInfo? propertyInfo = result.GetType().GetGenericArguments()[0].GetProperty(SortColumnConstants.DefaultSortColumn);
                if (propertyInfo != null)
                    result = result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, SortColumnConstants.DefaultSortColumn));
            }

            return await result.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(Id, cancellationToken);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(whereCondition, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? whereCondition, int pageIndex = 0, int numberItemsPerPage = 0, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
            var elementType = result.ElementType;
            var isDateAuditable = typeof(IDateAudited).IsAssignableFrom(elementType);

            if (whereCondition != null)
                result = result.Where(whereCondition);

            if (sortCriteria.Any())
            {
                bool isFirstSortQuery = true;
                foreach (var criteria in sortCriteria)
                {
                    var sortColumnName = criteria.Item1;
                    var sortDirection = criteria.Item2;

                    if (isFirstSortQuery)
                    {
                        isFirstSortQuery = false;
                        result = sortDirection == SortDirection.Ascending ?
                            result.OrderBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName)) :
                            result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                    else
                    {
                        result = sortDirection == SortDirection.Ascending
                            ? ((IOrderedQueryable<TEntity>)result).ThenBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName))
                            : ((IOrderedQueryable<TEntity>)result).ThenByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                }
            }
            else
            {
                PropertyInfo? propertyInfo = result.GetType().GetGenericArguments()[0].GetProperty(SortColumnConstants.DefaultSortColumn);
                if (propertyInfo != null)
                    result = result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, SortColumnConstants.DefaultSortColumn));
            }

            if (pageIndex == 0 && numberItemsPerPage == 0)
                return await result.AsNoTracking().ToListAsync(cancellationToken);

            return await result.Skip((pageIndex + 1) * numberItemsPerPage)
                .Take(numberItemsPerPage)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class EFClassRepository<TDbContext, TEntity, TPrimaryKey> : IEFClassRepository<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly TDbContext _dbContext;
        public EFClassRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ISoftDeleted)
            {
                ((ISoftDeleted)entity).IsDeleted = false;
                ((ISoftDeleted)entity).DeletedAt = null;
            }

            if (entity is IActiveEntity)
            {
                ((IActiveEntity)entity).IsActive = true;
            }
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.FirstOrDefault() is IDateAudited)
            {
                foreach (var entity in entities)
                {
                    if (entity is IDateAudited)
                    {
                        ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                        ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
                    }

                    if (entity is ISoftDeleted)
                    {
                        ((ISoftDeleted)entity).IsDeleted = false;
                        ((ISoftDeleted)entity).DeletedAt = null;
                    }

                    if (entity is IActiveEntity)
                    {
                        ((IActiveEntity)entity).IsActive = true;
                    }
                }
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            if (whereCondition == null)
                return await _dbContext.Set<TEntity>().CountAsync(cancellationToken);

            return await _dbContext.Set<TEntity>().CountAsync(whereCondition, cancellationToken);
        }

        public async Task HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task HardDeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> whereCondition, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
            var elementType = result.ElementType;
            var isDateAuditable = typeof(IDateAudited).IsAssignableFrom(elementType);

            if (whereCondition != null)
                result = result.Where(whereCondition);

            if (sortCriteria.Any())
            {
                bool isFirstSortQuery = true;
                foreach (var criteria in sortCriteria)
                {
                    var sortColumnName = criteria.Item1;
                    var sortDirection = criteria.Item2;

                    if (isFirstSortQuery)
                    {
                        isFirstSortQuery = false;
                        result = sortDirection == SortDirection.Ascending ?
                            result.OrderBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName)) :
                            result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                    else
                    {
                        result = sortDirection == SortDirection.Ascending
                            ? ((IOrderedQueryable<TEntity>)result).ThenBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName))
                            : ((IOrderedQueryable<TEntity>)result).ThenByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                }
            }
            else
            {
                PropertyInfo? propertyInfo = result.GetType().GetGenericArguments()[0].GetProperty(SortColumnConstants.DefaultSortColumn);
                if (propertyInfo != null)
                    result = result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, SortColumnConstants.DefaultSortColumn));
            }

            return await result.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(Id, cancellationToken);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(whereCondition, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereCondition, int pageIndex = 0, int numberItemsPerPage = 0, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
            var elementType = result.ElementType;
            var isDateAuditable = typeof(IDateAudited).IsAssignableFrom(elementType);

            if (whereCondition != null)
                result = result.Where(whereCondition);

            if (sortCriteria.Any())
            {
                bool isFirstSortQuery = true;
                foreach (var criteria in sortCriteria)
                {
                    var sortColumnName = criteria.Item1;
                    var sortDirection = criteria.Item2;

                    if (isFirstSortQuery)
                    {
                        isFirstSortQuery = false;
                        result = sortDirection == SortDirection.Ascending ?
                            result.OrderBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName)) :
                            result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                    else
                    {
                        result = sortDirection == SortDirection.Ascending
                            ? ((IOrderedQueryable<TEntity>)result).ThenBy(x => RefectionExtensions.GetPropertyValue(x, sortColumnName))
                            : ((IOrderedQueryable<TEntity>)result).ThenByDescending(x => RefectionExtensions.GetPropertyValue(x, sortColumnName));
                    }
                }
            }
            else
            {
                PropertyInfo? propertyInfo = result.GetType().GetGenericArguments()[0].GetProperty(SortColumnConstants.DefaultSortColumn);
                if (propertyInfo != null)
                    result = result.OrderByDescending(x => RefectionExtensions.GetPropertyValue(x, SortColumnConstants.DefaultSortColumn));
            }

            if (pageIndex == 0 && numberItemsPerPage == 0)
                return await result.AsNoTracking().ToListAsync(cancellationToken);

            return await result.Skip((pageIndex + 1) * numberItemsPerPage)
                .Take(numberItemsPerPage)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
            if (entity != null)
                _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task HardDeleteRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            var entities = await _dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
            if (entities.Any())
                _dbContext.Set<TEntity>().RemoveRange(entities);
        }
    }

    public class EFRepository<TDbContext, TEntity, TPrimaryKey, TModel> : IEFRepository<TDbContext, TEntity, TPrimaryKey, TModel>
                where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TModel : class
    {
        protected readonly TDbContext _dbContext;
        private readonly IMapper _mapper;
        public EFRepository(TDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TEntity>(model);
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ISoftDeleted)
            {
                ((ISoftDeleted)entity).IsDeleted = false;
                ((ISoftDeleted)entity).DeletedAt = null;
            }

            if (entity is IActiveEntity)
            {
                ((IActiveEntity)entity).IsActive = true;
            }
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

            return _mapper.Map<TModel>(entity);
        }

        public async Task<IEnumerable<TModel>> AddRangeAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
        {
            var entities = _mapper.Map<List<TEntity>>(models);
            if (entities.FirstOrDefault() is IDateAudited)
            {
                foreach (var entity in entities)
                {
                    if (entity is IDateAudited)
                    {
                        ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                        ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
                    }

                    if (entity is ISoftDeleted)
                    {
                        ((ISoftDeleted)entity).IsDeleted = false;
                        ((ISoftDeleted)entity).DeletedAt = null;
                    }

                    if (entity is IActiveEntity)
                    {
                        ((IActiveEntity)entity).IsActive = true;
                    }
                }
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return _mapper.Map<List<TModel>>(entities);
        }

        public Task<int> CountAsync(Expression<Func<TModel, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(List<TModel> models, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TModel, bool>> whereCondition, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetFirstAsync(Expression<Func<TModel, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereCondition, int pageIndex = 0, int numberItemsPerPage = 0, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    public class EFClassRepository<TDbContext, TEntity, TPrimaryKey, TModel> : IEFClassRepository<TDbContext, TEntity, TPrimaryKey, TModel>
               where TDbContext : DbContext
       where TEntity : EntityBase<TPrimaryKey>
       where TModel : class
    {
        protected readonly TDbContext _dbContext;
        private readonly IMapper _mapper;
        public EFClassRepository(TDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TEntity>(model);
            if (entity is IDateAudited)
            {
                ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ISoftDeleted)
            {
                ((ISoftDeleted)entity).IsDeleted = false;
                ((ISoftDeleted)entity).DeletedAt = null;
            }

            if (entity is IActiveEntity)
            {
                ((IActiveEntity)entity).IsActive = true;
            }

            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

            return _mapper.Map<TModel>(entity);
        }

        public async Task<IEnumerable<TModel>> AddRangeAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
        {
            var entities = _mapper.Map<List<TEntity>>(models);
            if (entities.FirstOrDefault() is IDateAudited)
            {
                foreach (var entity in entities)
                {
                    if (entity is IDateAudited)
                    {
                        ((IDateAudited)entity).CreatedDate = DateTime.UtcNow;
                        ((IDateAudited)entity).UpdatedDate = DateTime.UtcNow;
                    }

                    if (entity is ISoftDeleted)
                    {
                        ((ISoftDeleted)entity).IsDeleted = false;
                        ((ISoftDeleted)entity).DeletedAt = null;
                    }

                    if (entity is IActiveEntity)
                    {
                        ((IActiveEntity)entity).IsActive = true;
                    }
                }
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return _mapper.Map<List<TModel>>(entities);
        }

        public Task<int> CountAsync(Expression<Func<TModel, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(List<TModel> models, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TModel, bool>> whereCondition, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetFirstAsync(Expression<Func<TModel, bool>> whereCondition = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereCondition, int pageIndex = 0, int numberItemsPerPage = 0, List<Tuple<string, SortDirection>> sortCriteria = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
