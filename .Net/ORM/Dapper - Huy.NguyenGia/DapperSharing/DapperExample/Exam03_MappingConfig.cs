using Dapper;
using DapperSharing.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSharing.DapperExample
{
    public static class Exam03_MappingConfig
    {

        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine("=========== RUNNING E03_MappingConfig ===========");
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
            public string ProductName { get; set; }
            public string product_name { get; set; }
        }

        static async Task QueryDefault(IDbConnection connection)
        {
            var sql = @"SELECT product_name
                        FROM Product
                        WHERE Id = 20";

            var products = await connection
                .QueryAsync<DefaultProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion


        #region Matching underscores
        class MatchingUnderscoresProductModel
        {
            public string ProductName { get; set; }
        }

        static async Task QueryMatchingUnderscores(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var sql = @"SELECT product_name
                        FROM Product
                        WHERE Id = 1";

            var products = await connection
                .QueryAsync<MatchingUnderscoresProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion

        #region Custom mapping
        class CustomMappingProductModel
        {
            public string CustomProductName { get; set; }
        }

        static async Task QueryCustomMapping(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = false;

            var customMap = new CustomPropertyTypeMap(
                typeof(CustomMappingProductModel),
                (type, columnName) =>
                {
                    if (columnName == "product_name")
                        return type.GetProperty(nameof(CustomMappingProductModel.CustomProductName));

                    return null;
                }
            );

            Dapper.SqlMapper.SetTypeMap(typeof(CustomMappingProductModel), customMap);

            var sql = @"SELECT * FROM Product
                        WHERE Id = 20";

            var products = await connection.QueryAsync<CustomMappingProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion
    }
}
