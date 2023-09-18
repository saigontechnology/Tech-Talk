using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public interface IProductRepository
{
    Task<List<ProductInformation>> GetProductsAsync(string? name, CancellationToken cancellationToken = default);
    Task<bool> CheckProductNameExistsAsync(string productName, Guid? productId = null, CancellationToken cancellationToken = default);
    Task<bool> CheckProductExistsAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<CreateProductResponse> CreateProductAsync(CreateProduct createProduct, CancellationToken cancellationToken = default);
    Task<UpdateProductResponse> UpdateProductAsync(UpdateProduct updateProduct, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default);
}