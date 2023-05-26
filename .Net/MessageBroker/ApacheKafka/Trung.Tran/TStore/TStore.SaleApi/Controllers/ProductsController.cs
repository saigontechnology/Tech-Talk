using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TStore.Shared.Models;
using TStore.Shared.Services;

namespace TStore.SaleApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetProducts([FromBody] SimpleFilterModel filter)
        {
            IEnumerable<ProductModel> products = await _productService.GetProductsAsync(filter);

            return Ok(products);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            await _productService.UpdateProductAsync(model);

            return NoContent();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel model)
        {
            await _productService.CreateProductAsync(model);

            return Ok(model);
        }
    }
}
