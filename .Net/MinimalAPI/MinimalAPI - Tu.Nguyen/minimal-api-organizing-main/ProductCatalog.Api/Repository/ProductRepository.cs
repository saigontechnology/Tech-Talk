using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Entities;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductCatalogDbContext _db;
    private readonly IMapper _mapper;
    
    public ProductRepository(ProductCatalogDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<List<ProductInformation>> GetProductsAsync(string? name, CancellationToken cancellationToken = default)
    {
        var productsQueryable = _db.Products.Where(c => c.IsActive);

        if (!string.IsNullOrEmpty(name))
            productsQueryable = productsQueryable.Where(c => c.Name.ToLower().Trim().Contains(name.ToLower().Trim()));

        var products = await productsQueryable
            .Include(product => product.Category)
            .Select(product => _mapper.Map<ProductInformation>(product))
            .AsNoTracking().ToListAsync(cancellationToken);
        
        return products;
    }

    public async Task<bool> CheckProductNameExistsAsync(string productName, Guid? productId = null, CancellationToken cancellationToken = default)
    {
        var productsQueryable = _db.Products.AsNoTracking();

        if (productId is not null)
            productsQueryable = productsQueryable.Where(product => product.Id != productId);

        return await productsQueryable.AnyAsync(p => p.Name.ToLower().Trim().Equals(productName.ToLower().Trim()), cancellationToken);
    }

    public async Task<bool> CheckProductExistsAsync(Guid productId, CancellationToken cancellationToken = default)
        => await _db.Products.AnyAsync(p => p.Id == productId);

    public async Task<CreateProductResponse> CreateProductAsync(CreateProduct createProduct, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(createProduct);

        await _db.Products.AddAsync(product, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreateProductResponse>(product);
    }

    public async Task<UpdateProductResponse> UpdateProductAsync(UpdateProduct updateProduct, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(updateProduct);

        _db.Entry(product).Property(p => p.Name).IsModified = true;
        _db.Entry(product).Property(p => p.Price).IsModified = true;
        _db.Entry(product).Property(p => p.StockQuantity).IsModified = true;
        _db.Entry(product).Property(p => p.Description).IsModified = true;
        _db.Entry(product).Property(p => p.CategoryId).IsModified = true;
        _db.Entry(product).Property(p => p.UpdatedBy).IsModified = true;
        _db.Entry(product).Property(p => p.UpdatedDate).IsModified = true;
        
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UpdateProductResponse>(product);
    }
    
    public async Task<bool> DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = new Product()
        {
            Id = productId,
            IsActive = false,
            UpdatedBy = "Admin",
            UpdatedDate = DateTime.UtcNow
        };

        _db.Entry(product).Property(p => p.IsActive).IsModified = true;
        _db.Entry(product).Property(p => p.UpdatedBy).IsModified = true;
        _db.Entry(product).Property(p => p.UpdatedDate).IsModified = true;
        
        return await _db.SaveChangesAsync(cancellationToken) > 0;
    }
}