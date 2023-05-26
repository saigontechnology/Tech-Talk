using BasicLinQ.Context;
using BasicLinQ.Operators;
using Microsoft.AspNetCore.Mvc;

namespace BasicLinQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : Controller
    {
        [HttpGet("deferred")]
        public IActionResult ExecutionDeferred()
        {
            Console.WriteLine("Deferred");
            using ApplicationDbContext context = new();
            var categories = context.ProductCategories;
            var categoriesQuery = categories.WhereExecution(x => x.Id <= 2);

            Console.WriteLine("Deferred Streaming");
            foreach (var category in categoriesQuery) {
                Console.WriteLine("Category Id: " + category.Id);
            }

            var listCategoriesFilterred = categoriesQuery.ToList();

            Console.WriteLine("\nDeferred Non-Streaming 1");
            foreach (var category in listCategoriesFilterred)
            {
                Console.WriteLine("Category Id: " + category.Id);
            }

            Console.WriteLine();

            return Ok();
        }

        [HttpGet("immediate")]
        public IActionResult ExecutionImmediate()
        {
            Console.WriteLine("Immediate");
            using ApplicationDbContext context = new();
            var categories = context.ProductCategories;

            var categoriesQuery = categories.WhereExecution(x => x.Id <= 2);
            var listCategoriesFilterred = categoriesQuery.ToList();

            Console.WriteLine("\nDeferred Non-Streaming 1");
            foreach (var category in categoriesQuery)
            {
                Console.WriteLine("Category Id: " + category.Id);
            }

            Console.WriteLine("\nDeferred Non-Streaming 1");
            foreach (var category in categoriesQuery)
            {
                Console.WriteLine("Category Id: " + category.Id);
            }

            Console.WriteLine("Immediate 1");
            foreach (var category in listCategoriesFilterred)
            {
                Console.WriteLine("Category Id: " + category.Id);
            }

            Console.WriteLine("Immediate 2");
            foreach (var category in listCategoriesFilterred)
            {
                Console.WriteLine("Category Id: " + category.Id);
            }
            Console.WriteLine();

            return Ok();
        }
    }
}
