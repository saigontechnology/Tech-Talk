using Dapper;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DapperSharing.Examples
{
    public static class E02_QueryData
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("=========== RUNNING E02_QueryData ===========");
                DisplayHelper.PrintListOfMethods(typeof(E02_QueryData));
                //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var userInput = Console.ReadLine();
                using var connection = new SqlConnection(Program.DBInfo.ConnectionString);
                switch (userInput)
                {
                    case "1":
                        QuerySingleRow(connection);
                        break;
                    case "2":
                        QueryMultipleRows(connection);
                        break;
                    case "3":
                        QueryMultiResults(connection);
                        break;
                    case "4":
                        QuerySpecificColumns(connection);
                        break;
                    case "5":
                        QueryScalarValues(connection);
                        break;
                    case "b":
                        return;
                }
            }
        }

        static void QuerySingleRow(IDbConnection connection)
        {
            var sql = @"SELECT TOP 10 * FROM production.products
                        WHERE ModelYear = 3000";

            var entity = connection.QueryFirst<Product>(sql);
            DisplayHelper.PrintJson(entity);
        }

        static void QueryMultipleRows(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products
                        WHERE BrandId = 4";

            var results = connection.Query<Product>(sql);

            DisplayHelper.PrintJson(results);
        }

        static void QueryMultiResults(IDbConnection connection)
        {
            var sql =
                @"
                SELECT * FROM production.products WHERE ProductId = 1;
                SELECT * FROM production.products WHERE BrandId = 4;";

            using var multi = connection.QueryMultiple(sql);
            var entity = multi.ReadFirstOrDefault<Product>();
            DisplayHelper.PrintJson(entity);

            var results = multi.Read<Product>();
            DisplayHelper.PrintJson(results);
        }

        static void QuerySpecificColumns(IDbConnection connection)
        {
            var sql = @"SELECT ProductId, ProductName FROM production.products 
                        WHERE ProductId = 1";

            var entity = connection.QueryFirstOrDefault<Product>(sql);
            DisplayHelper.PrintJson(entity);
            
            var entity1 = connection.QueryFirstOrDefault(sql);
            DisplayHelper.PrintJson(entity1);
        }

        static void QueryScalarValues(IDbConnection connection)
        {
            var sql = @"SELECT COUNT(*) FROM production.products";

            var count = connection.ExecuteScalar<int>(sql);

            Console.WriteLine($"Product count: {count}");
        }
    }
}
