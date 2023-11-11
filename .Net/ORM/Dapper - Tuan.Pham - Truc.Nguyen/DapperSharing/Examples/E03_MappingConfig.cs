using Dapper;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperSharing.Examples
{
    public static class E03_MappingConfig
    {
        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine("=========== RUNNING E03_MappingConfig ===========");
                DisplayHelper.PrintListOfMethods(typeof(E03_MappingConfig));
                using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
                {
                    var userInput = Console.ReadLine();
                    switch (userInput)
                    {
                        case "1":
                            await QueryDefault(connection);
                            break;
                        case "2":
                            await QueryMatchingUnderscores(connection);
                            break;
                        case "3":
                            await QueryCustomMapping(connection);
                            break;
                        case "b":
                            return;
                    }
                }
            }
        }

        #region Default
        class DefaultProductModel
        {
            public int ProductId { get; set; }
            public int product_id { get; set; }
        }

        static async Task QueryDefault(IDbConnection connection)
        {
            var sql = @"SELECT ProductId AS product_id 
                        FROM production.products
                        WHERE ProductId = 1";

            var products = await connection
                .QueryAsync<DefaultProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion

        #region Matching underscores
        class MatchingUnderscoresProductModel
        {
            public int ProductId { get; set; }
        }

        static async Task QueryMatchingUnderscores(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var sql = @"SELECT ProductId AS product_id 
                        FROM production.products
                        WHERE ProductId = 1";

            var products = await connection
                .QueryAsync<MatchingUnderscoresProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion

        #region Custom mapping
        class CustomMappingProductModel
        {
            public int CustomProductId { get; set; }
        }

        static async Task QueryCustomMapping(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = false;

            var customMap = new CustomPropertyTypeMap(
                typeof(CustomMappingProductModel),
                (type, columnName) =>
                {
                    if (columnName == "ProductId")
                        return type.GetProperty(nameof(CustomMappingProductModel.CustomProductId));

                    return null;
                }
            );

            Dapper.SqlMapper.SetTypeMap(typeof(CustomMappingProductModel), customMap);

            var sql = @"SELECT * FROM production.products
                        WHERE ProductId = 1";

            var products = await connection.QueryAsync<CustomMappingProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion
    }
}
