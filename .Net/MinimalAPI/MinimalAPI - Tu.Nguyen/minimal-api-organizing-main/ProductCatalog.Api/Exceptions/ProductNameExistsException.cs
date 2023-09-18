namespace ProductCatalog.Api.Exceptions;

public class ProductNameExistsException : Exception
{
    public ProductNameExistsException(string productName)
        : base($"ProductName: {productName} already exists.")
    {
    }
}