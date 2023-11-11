using Dapper;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DapperSharing.Examples
{
    public static class E06_Relationships
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E06_Relationships ===========");
            DisplayHelper.PrintListOfMethods(typeof(E06_Relationships));
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        await QueryOneToMany(connection);
                        break;
                    case "2":
                        await QueryManyToMany(connection);
                        break;
                    case "3":
                        await QueryMultipleRelationships(connection);
                        break;
                    case "4":
                        await QueryOneToManyEFCore();
                        break;
                    case "5":
                        await QueryOneToManyDapperBasic(connection);
                        break;
                }
            }
        }

        static async Task QueryOneToMany(IDbConnection connection)
        {
            var sql = @"
                SELECT p.ProductId, p.ProductName, c.CategoryId, c.CategoryName
                FROM production.products p
                INNER JOIN production.categories c ON p.CategoryId = c.CategoryId
                WHERE ProductId = 1";

            var result = await connection.QueryAsync<Product, Category, Product>(sql,
                (product, category) =>
                {
                    product.Category = category;
                    return product;
                }, splitOn: "CategoryId");

            DisplayHelper.PrintJson(result);
        }

        static async Task QueryManyToMany(IDbConnection connection)
        {
            var sql = @"
                SELECT 
                    s.store_id,
                    s.store_name,
                    p.ProductId, 
                    p.ProductName
                FROM sales.stores s
                INNER JOIN production.stocks st ON s.store_id = st.store_id
                INNER JOIN production.products p ON st.ProductId = p.ProductId;";

            var storeMap = new Dictionary<int, Category>();

            var result = await connection.QueryAsync<Category, Product, Category>(sql,
                (store, product) =>
                {
                    if (!storeMap.TryGetValue(store.CategoryId, out var cachedStore))
                    {
                        cachedStore = store;
                        cachedStore.Products ??= new List<Product>();
                        storeMap[cachedStore.CategoryId] = cachedStore;
                    }
                    cachedStore.Products.Add(product);
                    return cachedStore;
                }, splitOn: "ProductId");

            DisplayHelper.PrintJson(storeMap.Values);
        }

        static async Task QueryMultipleRelationships(IDbConnection connection)
        {
            var sql = @"
                SELECT 
                    s.BrandId,
                    s.BrandName,
                    p.ProductId, 
                    p.ProductName,
                    c.CategoryId,
                    c.CategoryName
                FROM sales.stores s
                INNER JOIN production.stocks st ON s.BrandId = st.BrandId
                INNER JOIN production.products p ON st.ProductId = p.ProductId
                INNER JOIN production.categories c ON p.CategoryId = c.CategoryId;";

            var storeMap = new Dictionary<int, Brand>();

            var result = await connection.QueryAsync<Brand, Product, Category, Brand>(sql,
                (brand, product, category) =>
                {
                    if (!storeMap.TryGetValue(brand.BrandId, out var cachedBrand))
                    {
                        cachedBrand = brand;
                        cachedBrand.Products ??= new List<Product>();
                        storeMap[brand.BrandId] = cachedBrand;
                    }
                    product.Category = category;
                    cachedBrand.Products.Add(product);
                    return cachedBrand;
                }, splitOn: "ProductId,CategoryId");

            DisplayHelper.PrintJson(storeMap.Values);
        }

        static async Task QueryOneToManyEFCore()
        {
            var connection = new BikeStoresContext(
                Program.DBInfo.ConnectionString);

            var result = await connection.Products
                .Where(x => x.ProductId == 1)
                .Include(x => x.Category)
                .FirstOrDefaultAsync();

            DisplayHelper.PrintJsonWithoutLoop(result);
        }

        static async Task QueryOneToManyDapperBasic(IDbConnection connection)
        {
            var sql = @"
                SELECT p.ProductId, p.ProductName FROM production.products p
		        WHERE ProductId = 1

	            SELECT c.CategoryId, c.CategoryName FROM production.categories c
		        WHERE CategoryId = (SELECT p.CategoryId
			        FROM production.products p
			        WHERE ProductId = 1)";

            var result = await connection.QueryMultipleAsync(sql);
            var product = result.ReadFirstOrDefault<Product>();
            product.Category = result.ReadFirstOrDefault<Category>();

            DisplayHelper.PrintJson(product);
        }
    }
}
