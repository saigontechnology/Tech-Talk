using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repository;

namespace ProductCatalog.Api.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryGroup(this RouteGroupBuilder group)
    {
        group.MapGet("", GetCategoriesAsync)
            .WithName("GetCategories")
            .WithSummary("Get all categories")
            .WithDescription("Get all active categories")
            .MapToApiVersion(1).MapToApiVersion(2);
        
        return group;
    }

    private static async Task<Results<Ok<List<CategoryInformation>>, NotFound>> GetCategoriesAsync(ICategoryRepository _categoryRepository,
        CancellationToken cancellationToken,
        [FromQuery] string? name)
    {
        var categories = await _categoryRepository.GetCategoriesAsync(name, cancellationToken);
        
        return TypedResults.Ok(categories);
    }
}