using Dapper;
using Dapper.Contrib.Extensions;
using DapperSharing.Helper;
using DapperSharing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace DapperSharing
{
    public static class Exam09_Extensions
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E09_Extensions ===========");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var userInput = Console.ReadLine();
            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                switch (userInput)
                {
                    case "1":
                        await SqlBuilder(connection);
                        break;

                    case "2":
                        await Contrib(connection);
                        break;
                    case "3":
                        await DapperPlus(connection);
                        break;
                }
            }
        }
        static async Task SqlBuilder(IDbConnection connection)
        {
            var builder = new SqlBuilder()
                .Select("p.*")
                .OrderBy("p.Size DESC, p.product_name ASC");

            var types = new List<Type>
            {
                typeof(Product)
            };
            var splitOns = new List<string>();

            Console.Write("Input search: ");
            var search = Console.ReadLine();

            Console.Write("Input Product Detail: ");
            var productDetail = Console.ReadLine();

            Console.Write("Input size: ");
            int.TryParse(Console.ReadLine(), out var size);

            if (!string.IsNullOrWhiteSpace(search))
            {
                builder.Where("p.product_name LIKE @Search", new
                {
                    Search = $"%{search}%"
                });
            }

            if (!string.IsNullOrWhiteSpace(productDetail))
            {
                builder.InnerJoin("ProductDetail as pd ON p.IdProductDetail = pd.Id")
                    .Select("pd.Id, pd.Name")
                    .Where("pd.Name LIKE @ProductDetailSearch", new
                    {
                        ProductDetailSearch = $"%{productDetail}%"
                    });

                types.Add(typeof(ProductDetail));
                splitOns.Add("Id");
            }

            if (size > 0)
            {
                builder.Where("p.Size = @Size", new
                {
                    Year = size
                });
            }

            var template = builder.AddTemplate(@$"
                                        SELECT
                                            /**select**/ 
                                        FROM Product AS p
                                            /**innerjoin**/
                                            /**where**/ 
                                            /**orderby**/");

            var categoryIdx = types.IndexOf(typeof(ProductDetail));
            var products = await connection.QueryAsync(template.RawSql, types: types.ToArray(), map: (data) =>
            {
                var product = data[0] as Product;
                if (categoryIdx > -1)
                {
                    var productDetail = data[categoryIdx] as ProductDetail;
                    product.ProductDetail = productDetail;
                }
                return product;
            }, param: template.Parameters, splitOn: string.Join(',', splitOns));

            DisplayHelper.PrintJson(products);
        }


        static async Task Contrib(IDbConnection connection)
        {
            var product = new Product
            {
                IsDeleted = false,
                Size = 40,
                Quantity = 10,
                IdProductDetail = 30,
                ProductName = "Shoes in 2023"
            };
            var productIdContrib = await connection.InsertAsync(product);
            Console.WriteLine("Product Id by Dapper.Contrib: " + productIdContrib);

            var rawSql = @"
                INSERT INTO Product
                    (product_name, Quantity, Size, IdProductDetail, IsDeleted)
                VALUES 
                    (@ProductName, @Quantity, @Size, @IdProductDetail, @IsDeleted);
                SELECT SCOPE_IDENTITY()";

            var productIdNormal = await connection.ExecuteScalarAsync(rawSql, product);
            Console.WriteLine("Product Id by Dapper Normal: " + productIdNormal);
        }

        static async Task DapperPlus(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var sql = @"SELECT TOP 10 * FROM Product
                        ORDER BY Id DESC";
            var products = connection.Query<Product>(sql)
                .Select(x => new Product
                {
                    IsDeleted = false,
                    Quantity = x.Quantity,
                    Size = x.Size,
                    IdProductDetail = x.IdProductDetail,
                    ProductName = x.ProductName + "_DapperPlus"
                });

            var sqlInsert = @"
                INSERT INTO Product
                    (product_name, Quantity, Size, IdProductDetail, IsDeleted)
                VALUES 
                    (@ProductName, @Quantity, @Size, @IdProductDetail, @IsDeleted);";

            var resultNormal = connection.Execute(sqlInsert, products);

            var result = connection.BulkInsert(products);
            IEnumerable<Product> productInserted = result.Current;
        }
    }
}
