using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductCatalogDbContext _db;
    private readonly IMapper _mapper;
    
    public CategoryRepository(ProductCatalogDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<List<CategoryInformation>> GetCategoriesAsync(string? name, CancellationToken cancellationToken = default)
    {
        var categoriesQueryable = _db.Categories.Where(c => c.IsActive);

        if (!string.IsNullOrEmpty(name))
            categoriesQueryable = categoriesQueryable.Where(c => c.Name.ToLower().Contains(name.ToLower()));
        
        return _mapper.Map<List<CategoryInformation>>(await categoriesQueryable.AsNoTracking().ToListAsync(cancellationToken));
    }
}