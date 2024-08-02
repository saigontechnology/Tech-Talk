using Microsoft.AspNetCore.Mvc;
using SharedDomains;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ILogger<CategoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<IdNameModel<ProductCategoryType>> Get()
    {
        var result = Enum.GetValues<ProductCategoryType>();

        return result.Select(x => new IdNameModel<ProductCategoryType>(x, x.ToString()));
    }
}
