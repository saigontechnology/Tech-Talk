using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDomains;
using WebAPI.Second;

namespace WebApi.Second.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController([FromKeyedServices(DomainConst.HTTP_CLIENT_STORE)] HttpClient storeApi,
    ProductContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<List<ProductModel>> Get(CancellationToken cancellationToken)
    {
        var result = await dbContext.Products.ToListAsync(cancellationToken);

        if (!result.Any())
        {
            result = [.. Faker.RandomNumber.Next(10, 20).Generate<ProductModel>()];
            await dbContext.Products.AddRangeAsync(
                result,
                cancellationToken
            );

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    [HttpGet]
    [Route("categories")]
    public async Task<List<IdNameModel<ProductCategoryType>>> GetCategories(CancellationToken cancellationToken)
    {
        int[] errorNums = [3, 5];
        if (errorNums.Contains(Faker.RandomNumber.Next(5)))
        {
            throw new ApplicationException("Cannot fetch categories");
        }

        return await storeApi.GetFromJsonAsync<List<IdNameModel<ProductCategoryType>>>("/category", cancellationToken) ?? [];
    }

    [HttpGet]
    [Route("migrate")]
    public async Task Migrate(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
