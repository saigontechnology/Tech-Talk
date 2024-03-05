using C_11_Demo.Models;

namespace C_11_Demo.Pieces.Validator;
internal class ProductService
{
    [MyValidation<AddProductValidator>()]
    public void Add(Product product)
    {

    }

    [MyValidation<UpdateProductValidator>()]
    public void Update(Product product)
    {

    }

    public void UpdateNoValidation(Product product)
    {

    }
}

internal class FactoryService
{
    private readonly ProductService _productService;

    public FactoryService()
    {
        _productService = new();
    }

    public void AddProduct()
    {
        var product = new Product
        {
            Name = "Name",
            Age = 10
        };

        Invoker.InvokeAction(_productService.Add, product);
    }

    public void UpdateProduct()
    {
        var product = new Product
        {
            Name = "",
            Age = 10
        };

        Invoker.InvokeAction(_productService.Update, product);
    }

    public void UpdateProductNoValidation()
    {
        var product = new Product
        {
            Name = "",
            Age = 10
        };

        Invoker.InvokeAction(_productService.UpdateNoValidation, product);
    }
}