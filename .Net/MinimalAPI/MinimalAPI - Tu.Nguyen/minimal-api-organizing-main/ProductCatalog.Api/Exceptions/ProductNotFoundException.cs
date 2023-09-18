namespace ProductCatalog.Api.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid productId)
        : base($"ProductId: {productId} was not found.")
    {
    }
}