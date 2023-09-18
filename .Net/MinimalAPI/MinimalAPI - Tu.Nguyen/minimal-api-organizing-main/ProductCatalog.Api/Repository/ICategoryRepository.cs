using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public interface ICategoryRepository
{
    Task<List<CategoryInformation>> GetCategoriesAsync(string? name, CancellationToken cancellationToken = default);
}