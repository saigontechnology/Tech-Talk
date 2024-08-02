using Microsoft.AspNetCore.Mvc;
using SharedDomains;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly HttpClient productApi = httpClientFactory.CreateClient(DomainConst.HTTP_CLIENT_PRODUCT);
    private const string Route_Product = "/product";

    [HttpGet]
    public async Task<List<StoreModel>> Get(CancellationToken cancellationToken)
    {
        var stores = Enum.GetValues<ProductCategoryType>()
            .Select(x =>
            {
                var store = new StoreModel();
                store.Generate();
                store.Category = x;
                return store;
            })
            .ToList();

        var products = await productApi.GetFromJsonAsync<ProductModel[]>(Route_Product, cancellationToken);

        (
            from store in stores
            join product in products on store.Category equals product.Category into productTemp
            from product in productTemp.DefaultIfEmpty()
            select (store, product)
        )
        .Select(x =>
        {
            var (store, product) = x;
            store.Products ??= new List<ProductModel>();

            store.Products = store.Products.Append(product);

            return store;
        })
        .ToList();

        return stores;
    }
}
