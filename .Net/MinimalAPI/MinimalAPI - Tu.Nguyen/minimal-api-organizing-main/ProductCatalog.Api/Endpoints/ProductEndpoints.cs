using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Exceptions;
using ProductCatalog.Api.Filters;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repository;

namespace ProductCatalog.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductGroup(this RouteGroupBuilder group)
    {
        group.MapGet("", GetProductsAsync)
            .WithName("GetProducts")
            .WithSummary("Search all products")
            .WithDescription("Get all active products")
            .MapToApiVersion(1);

        group.MapPost("", CreateProductAsync)
            .WithName("CreateProduct")
            .WithSummary("Create product")
            .WithDescription("Create a new product")
            .AddEndpointFilter<ValidationFilter<CreateProduct>>();

        group.MapPut("", UpdateProductAsync)
            .WithName("UpdateProduct")
            .WithSummary("Update product")
            .WithDescription("Update a existing product")
            .AddEndpointFilter<ValidationFilter<UpdateProduct>>();

        group.MapDelete("", DeleteProductAsync)
            .WithName("DeleteProduct")
            .WithSummary("Delete product")
            .WithDescription("Delete a existing product");

        return group;
    }

    private static async Task<Results<Ok<List<ProductInformation>>, BadRequest>> GetProductsAsync(IProductRepository _productRepository,
        CancellationToken cancellationToken,
        [FromQuery] string? name)
    {
        var products = await _productRepository.GetProductsAsync(name, cancellationToken);
        
        return TypedResults.Ok(products);
    }
    
    private static async Task<Results<Ok<CreateProductResponse>, BadRequest>> CreateProductAsync(IProductRepository _productRepository,
        CancellationToken cancellationToken,
        [FromBody] CreateProduct createProduct)
    {
        var isProductNameExists = await _productRepository.CheckProductNameExistsAsync(productName: createProduct.Name, cancellationToken: cancellationToken);
        
        if (isProductNameExists)
            throw new ProductNameExistsException(createProduct.Name);

        var createProductResponse = await _productRepository.CreateProductAsync(createProduct, cancellationToken);
        
        return TypedResults.Ok(createProductResponse);
    }
    
    private static async Task<Results<Ok<UpdateProductResponse>, BadRequest>> UpdateProductAsync(IProductRepository _productRepository,
        CancellationToken cancellationToken,
        [FromBody] UpdateProduct updateProduct)
    {
        var isProductExists = await _productRepository.CheckProductExistsAsync(updateProduct.Id, cancellationToken);

        if (!isProductExists)
            throw new ProductNotFoundException(updateProduct.Id);

        var isProductNameExists = await _productRepository.CheckProductNameExistsAsync(updateProduct.Name, updateProduct.Id, cancellationToken);
        
        if (isProductNameExists)
            throw new ProductNameExistsException(updateProduct.Name);

        var updateProductResponse = await _productRepository.UpdateProductAsync(updateProduct, cancellationToken);
        
        return TypedResults.Ok(updateProductResponse);
    }
    
    private static async Task<Results<Ok<bool>, NotFound>> DeleteProductAsync(IProductRepository _productRepository,
        CancellationToken cancellationToken,
        [FromQuery] Guid productId)
    {
        var isProductExists = await _productRepository.CheckProductExistsAsync(productId, cancellationToken);

        if (!isProductExists)
            throw new ProductNotFoundException(productId);

        var deleteProductResponse = await _productRepository.DeleteProductAsync(productId, cancellationToken);
        
        return TypedResults.Ok(deleteProductResponse);
    }
}