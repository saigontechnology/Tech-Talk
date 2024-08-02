using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SharedDomains;

namespace ProductApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(ILogger<ProductController> logger) : ControllerBase
{
    [HttpGet]
    [OutputCache(Duration = 2)]
    public async Task<List<ProductModel>> Get(CancellationToken cancellationToken)
    {
        Console.WriteLine("Get Products console");
        logger.LogInformation("Get Product logger");
        return [.. Faker.RandomNumber.Next(10, 20).Generate<ProductModel>()];
    }
}
