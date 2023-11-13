using Dapper;
using Dapper.Contrib.Extensions;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using Z.Dapper.Plus;

namespace DapperSharing.Examples
{
    public static class E09_Extensions
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E09_Extensions ===========");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            DisplayHelper.PrintListOfMethods(typeof(E09_Extensions));

            var userInput = Console.ReadLine();
            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                switch (userInput)
                {
                    case "1":
                        await Contrib(connection);
                        break;
                    case "2":
                        await SqlBuilder(connection);
                        break;
                    case "3":
                        await DapperPlus(connection);
                        break;
                }
            }
        }

        static async Task Contrib(IDbConnection connection)
        {
            var product = new Product
            {
                BrandId = 1,
                CategoryId = 1,
                ListPrice = 150,
                ModelYear = 2023,
                ProductName = "My 2023 Product"
            };
            var productIdContrib = await connection.InsertAsync(product);
            Console.WriteLine("Product Id by Dapper.Contrib: " + productIdContrib);

            var rawSql = @"
                INSERT INTO production.products
                    (ProductName, BrandId, CategoryId, ModelYear, ListPrice)
                VALUES 
                    (@ProductName, @BrandId, @CategoryId, @ModelYear, @ListPrice);
                SELECT SCOPE_IDENTITY()";

            var productIdNormal = await connection.ExecuteScalarAsync(rawSql, product);
            Console.WriteLine("Product Id by Dapper Normal: " + productIdNormal);
        }

        static async Task SqlBuilder(IDbConnection connection)
        {
            Console.Write("Input search: ");
            var search = Console.ReadLine();

            Console.Write("Input category: ");
            var category = Console.ReadLine();

            Console.Write("Input model year: ");
            int.TryParse(Console.ReadLine(), out var modelYear);

            #region Raw SQL
            var sql = "SELECT TOP 10 * FROM production.products AS p";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(search))
            {
                dynamicParameters.AddDynamicParams(new { Search = $"%{search}%" });
                sql += "\nWHERE p.ProductName LIKE @Search";
            }

            if (modelYear > 0)
            {
                dynamicParameters.AddDynamicParams(new { Year = modelYear });

                if (sql.Contains("WHERE"))
                {
                    sql += "\nAND p.ModelYear = @Year";
                }
                else
                {
                    sql += "\nWHERE p.ModelYear = @Year";
                }
            }

            var entity = connection.Query<Product>(sql, dynamicParameters);
            #endregion

            #region SqlBuilder

            var builder = new SqlBuilder()
                .Select("p.*")
                .OrderBy("p.ModelYear DESC, p.ProductName ASC");
            var types = new List<Type>
            {
                typeof(Product)
            };
            var splitOns = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                builder.Where("p.ProductName LIKE @Search", new
                {
                    Search = $"%{search}%"
                });
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                builder.InnerJoin("production.categories as c ON p.CategoryId = c.CategoryId")
                    .Select("c.CategoryId, c.CategoryName")
                    .Where("c.CategoryName LIKE @CategorySearch", new
                    {
                        CategorySearch = $"%{category}%"
                    });

                types.Add(typeof(Category));
                splitOns.Add("CategoryId");
            }

            if (modelYear > 0)
            {
                builder.Where("p.ModelYear = @Year", new
                {
                    Year = modelYear
                });
            }

            var template = builder.AddTemplate(@$"
                SELECT
                    /**select**/ 
                FROM production.products AS p
                    /**innerjoin**/
                    /**where**/ 
                    /**orderby**/");

            var categoryIdx = types.IndexOf(typeof(Category));
            var products = await connection.QueryAsync(template.RawSql, types: types.ToArray(), map: (data) =>
            {
                var product = data[0] as Product;

                if (categoryIdx > -1)
                {
                    var category = data[categoryIdx] as Category;
                    product.Category = category;
                }

                return product;
            }, param: template.Parameters, splitOn: string.Join(',', splitOns));

            #endregion
            DisplayHelper.PrintJson(products);
        }

        static async Task DapperPlus(IDbConnection connection)
        {
            var sql = @"SELECT TOP 13 * FROM production.products
                        ORDER BY ProductId DESC";
            var products = connection.Query<Product>(sql)
                .Select(x => new Product
                {
                    BrandId = x.BrandId,
                    CategoryId = x.CategoryId,
                    ListPrice = x.ListPrice + 5,
                    ModelYear = x.ModelYear,
                    ProductName = x.ProductName + "_DapperPlus"
                });

            var sqlInsert = @"
                INSERT INTO production.products
                    (ProductName, BrandId, CategoryId, ModelYear, ListPrice)
                VALUES 
                    (@ProductName, @BrandId, @CategoryId, @ModelYear, @ListPrice);";

            var resultNormal = connection.Execute(sqlInsert, products);

            var result = connection.BulkInsert(products);
            IEnumerable<Product> productInserted = result.Current;
        }
    }
}
