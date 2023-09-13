using Dapper;
using DapperSharing.Helper;
using DapperSharing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSharing.DapperExample
{
    public static class Exam06_Relationships
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E06_Relationships ===========");
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
                    //case "3":
                    //    await QueryMultipleRelationships(connection);
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
        public static async Task QueryOneToMany(IDbConnection connection)
        {
            var sql = @"
                SELECT p.Id, 
                       p.Name,
                       br.Id as BrandId,
                       br.Name
                FROM ProductType p
                INNER JOIN Brand br ON br.Id = p.IdBrand";

            var result = await connection.QueryAsync<ProductType, Brand, ProductType>(sql,
                (productType, brand) =>
                {
                    productType.Brand = brand;
                    return productType;
                }, splitOn: "BrandId");

            DisplayHelper.PrintJson(result);
        }

        static async Task QueryManyToMany(IDbConnection connection)
        {
            var sql = @"
               SELECT 
                    cus.Id as CustomerId,
                    cus.FullName,
                    p.Id as ProductDetailId, 
                    p.Name
                FROM Customer cus
                INNER JOIN FavoriteProduct fp ON cus.Id = fp.IdCustomer
                INNER JOIN ProductDetail p ON p.Id = fp.IdProductDetail;";

            var productTypeMap = new Dictionary<int, ProductType>();

            var result = await connection.QueryAsync<ProductType, ProductDetail, ProductType>(sql,
                (productType, productDetail) =>
                {
                    if (!productTypeMap.TryGetValue(productType.Id, out var cachedStore))
                    {
                        cachedStore = productType;
                        cachedStore.ProductDetails ??= new List<ProductDetail>();
                        productTypeMap[cachedStore.Id] = cachedStore;
                    }
                    cachedStore.ProductDetails.Add(productDetail);
                    return cachedStore;
                }, splitOn: "ProductDetailId");

            DisplayHelper.PrintJson(productTypeMap.Values);
        }

        static async Task QueryOneToManyDapperBasic(IDbConnection connection)
        {
            var sql = @"
                SELECT pt.Id, pt.Name FROM ProductType pt
		        WHERE Id = 1

	            SELECT br.Id, br.Name FROM Brand br
		        WHERE Id = (SELECT br.IdBrand
			        FROM ProductType pt
			        WHERE Id = 1)";

            var result = await connection.QueryMultipleAsync(sql);
            var productDetail = result.ReadFirstOrDefault<ProductType>();
            productDetail.Brand = result.ReadFirstOrDefault<Brand>();

            DisplayHelper.PrintJson(productDetail);
        }

        static async Task QueryOneToManyEFCore()
        {
            var connection = new ShopOnlineShoesContext(
                Program.DBInfo.ConnectionString);

            var result = await connection.ProductTypes
                .Where(x => x.Id == 10)
                .Include(x => x.Brand)
                .FirstOrDefaultAsync();

            DisplayHelper.PrintJsonWithoutLoop(result);
        }
    }
}
