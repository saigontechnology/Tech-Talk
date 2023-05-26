using BasicLinQ.Context;
using BasicLinQ.Entities;
using BasicLinQ.Models.Product;
using BasicLinQ.Operators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicLinQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery] ProductRequestModel model)
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Get list Product by filters:");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();

            var productsQuery = from prod in context.Products
                                select prod;
            Helper.LogListData(productsQuery);

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                productsQuery = productsQuery.WhereCustomize(x => x.Name.Contains(model.SearchTerm));
                Helper.LogListData(productsQuery);
            }

            if (model.IsSortSimple)
            {
                productsQuery = productsQuery.AscendingSort();
                Helper.LogListData(productsQuery);
            }

            if (model.IsSortUseThenBy)
            {
                productsQuery = productsQuery.DescendingThenByAscendingSort();
                Helper.LogListData(productsQuery);
            }

            #region Log last execution
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Last execution:");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion
            IEnumerable<Product> result = productsQuery.Skip(model.Skip).Take(model.Take).ToList();

            return Ok(result);
        }

        [HttpPost("product-by-categories-id")]
        public IActionResult GetProductByCategoriesId([FromBody] int[] categoriesId)
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Get list Supplier is offering:");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();

            var productsJoin = context.ProductCategories.Get()
                .Join(context.Products.Get(),
                    cateProd => cateProd.Id,
                    prod => prod.CategoryId,
                    (cateProd, prod) => prod)
                .Where(x => categoriesId.Contains(x.CategoryId))
                .Select(x => new
                {
                    x.Id,
                    x.IsDeleted,
                    x.Name,
                    x.SupplierId,
                    x.CategoryId
                })
                .ToList();
            Helper.LogListData(productsJoin);

            var productsGroupBy = context.Products
                .Where(x => categoriesId.Contains(x.CategoryId))
                .GroupBy(x => x.CategoryId)
                .Select(group => new
                {
                    group.Key,
                    Product = group.Select(x => new
                    {
                        x.Id,
                        x.IsDeleted,
                        x.Name,
                        x.SupplierId,
                        x.CategoryId
                    })
                })
                .AsEnumerable()
                .SelectMany(x => x.Product)
                .ToList();
            Helper.LogListData(productsGroupBy);

            var productsCategoriesQuery = context.ProductCategories
                .Where(x => categoriesId.Contains(x.Id));

            var productsSelectMany = productsCategoriesQuery
                .SelectMany(x => x.Products.Select(x => new
                {
                    x.Id,
                    x.IsDeleted,
                    x.Name,
                    x.SupplierId,
                    x.CategoryId
                }))
                .ToList();
            Helper.LogListData(productsSelectMany);

            var productsSelect = productsCategoriesQuery
                .Select(x => x.Products.Select(x => new
                {
                    x.Id,
                    x.IsDeleted,
                    x.Name,
                    x.SupplierId,
                    x.CategoryId
                }))
                .ToList();

            return Ok();
        }

        [HttpGet("{supplierId}")]
        public IActionResult GetProductBySupplierId([FromRoute] int supplierId)
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Get Product by Supplier id:");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();

            Product detail = new();
            var productQuery = context.Products.AsQueryable();
            if (supplierId != 0)
            {
                productQuery = productQuery.Where(x => x.SupplierId == supplierId);
            }

            if (productQuery.CheckExisted())
            {
                try
                {
                    detail = productQuery.SingleAndSingleDefault();
                }
                catch (Exception ex)
                {
                    #region Log SingleOrDefault() ex
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("SingleOrDefault() exception: " + ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    #endregion 

                    detail = productQuery.FirstAndFirstDefault();
                }
            }
            else
            {
                detail = productQuery.FirstAndFirstDefault();
            }
            Helper.LogListData(new Product[] { detail });

            return Ok(detail);
        }

        [HttpGet("products-all-method")]
        public IActionResult CheckAllProductOfCategoryIsContainLetter(int categoryId, string letter)
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            var listProduct = productsQuery.ToList();

            //All
            var isContainAInProductEx1 = productsQuery.Where(x => x.CategoryId == categoryId).All(s => s.Name.Contains(letter));
            Console.WriteLine("All: " + isContainAInProductEx1);

            var isContainAInProductEx02 = listProduct.Where(x => x.CategoryId == categoryId).All(s => s.Name.Contains(letter));
            Console.WriteLine("All: " + isContainAInProductEx02);

            return Ok();
        }

        [HttpGet("products-distinct-name")]
        public IActionResult GetProductDistinctName(int categoryId, string letter)
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Distinct");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            var listProduct = productsQuery.ToList();

            //Distinct
            var listProductDistinct01 = productsQuery.Select(s => s.Name).Distinct();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("List Product Distint 01");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var product in listProductDistinct01)
            {
                Console.WriteLine(product);
            }

            var listCategoryDistinct02 = listProduct.Select(s => s.Name).Distinct();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("List Product Distint 02");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var product in listCategoryDistinct02)
            {
                Console.WriteLine(product);
            }

            return Ok();
        }

        [HttpGet("products-union-supplier")]
        public IActionResult GetProductUnionSupplier()
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Union");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            var listProduct = productsQuery.ToList();

            //Union
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Union");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductSupplier1 = productsQuery.Where(x => x.SupplierId == 1);
            var listProductSupplier2 = productsQuery.Where(x => x.SupplierId == 2);

            var listProductSupplierUnion = listProductSupplier1.Union(listProductSupplier2);
            Helper.LogListData(listProductSupplierUnion);

            var listProductSupplier01 = listProduct.Where(x => x.SupplierId == 1);
            var listProductSupplier02 = listProduct.Where(x => x.SupplierId == 2);

            var listProductSupplierUnion01 = listProductSupplier01.Union(listProductSupplier02);
            Helper.LogListData(listProductSupplierUnion01);

            return Ok();
        }

        [HttpGet("products-paging")]
        public IActionResult GetProductsPaging()
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Paging");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            var listProduct = productsQuery.ToList();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Skip");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductSkipWhile01 = productsQuery.OrderBy(x => x.Name).Skip(10);
            Helper.LogListData(listProductSkipWhile01);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("SkipWhile");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductSkipWhile02 = listProduct.SkipWhile(x => x.Name.Length > 10);
            Helper.LogListData(listProductSkipWhile02);

            //Take
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Take");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductTakeWhile01 = productsQuery.OrderBy(x => x.Name).Take(5);
            Helper.LogListData(listProductTakeWhile01);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("TakeWhile");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductTakeWhile02 = listProduct.TakeWhile(x => x.Name.Length > 5);
            Helper.LogListData(listProductTakeWhile02);

            return Ok();
        }

        [HttpGet("products-except")]
        public IActionResult GetListProductExcept()
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Paging");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            var listProduct = productsQuery.ToList();

            //Except
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Except");
            Console.ForegroundColor = ConsoleColor.Gray;
            var listProductExcept01 = productsQuery.Where(x => x.Name.Contains("a"));
            Helper.LogListData(listProductExcept01);
            var listProductExcept02 = productsQuery.Where(x => x.Name.Contains("e"));
            Helper.LogListData(listProductExcept02);
            var listProductExceptEx = listProductExcept01.Except(listProductExcept02);
            Helper.LogListData(listProductExceptEx);

            var listProductExcept03 = listProduct.Where(x => x.Name.Contains("a"));
            Helper.LogListData(listProductExcept03);
            var listProductExcept04 = listProduct.Where(x => x.Name.Contains("e"));
            Helper.LogListData(listProductExcept04);
            var listProductExcept = listProductExcept03.Except(listProductExcept04);
            Helper.LogListData(listProductExcept);

            return Ok();
        }

        [HttpGet("products-expression")]
        public IActionResult GetListProductUseExpression()
        {
            #region Log start get
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Expression");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            using ApplicationDbContext context = new();
            var productsQuery = context.Products.AsQueryable();

            Expression<Func<Product, bool>> isNameLength = s => s.Name.Length > 5 && s.Name.Length < 10;

            var listProduct = productsQuery.Where(isNameLength);

            Helper.LogListData(listProduct);

            ParameterExpression para = Expression.Parameter(typeof(Product), "s");
            MemberExpression me = Expression.Property(Expression.Property(para, "Name"), "Length");

            ConstantExpression constant1 = Expression.Constant(5, typeof(int));
            BinaryExpression body1 = Expression.GreaterThan(me, constant1);

            ConstantExpression constant2 = Expression.Constant(10, typeof(int));
            BinaryExpression body2 = Expression.LessThan(me, constant2);

            var expressionTree = Expression.Lambda<Func<Product, bool>>(Expression.AndAlso(body1,body2), new[] { para });

            Console.WriteLine("Expression Tree: {0}", expressionTree);

            Console.WriteLine("Expression Tree Body: {0}", expressionTree.Body);

            Console.WriteLine("Number of Parameters in Expression Tree: {0}",
                                            expressionTree.Parameters.Count);

            Console.WriteLine("Parameters in Expression Tree: {0}", expressionTree.Parameters[0]);

            return Ok();
        }


    }
}
