using Dapper;
using DapperSharing.Helper;
using DapperSharing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSharing.DapperExample
{
    public static class Exam02_QueryData
    {
        public static async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("=========== RUNNING E02_QueryData ===========");
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var userInput = Console.ReadLine();
                using var connection = new SqlConnection(Program.DBInfo.ConnectionString);
                switch (userInput)
                {
                    case "1":
                        await QuerySingleRow(connection);
                        break;
                    case "2":
                        await QueryMultipleRows(connection);
                        break;
                    case "3":
                        await QueryMultiResults(connection);
                        break;
                    case "4":
                        await QuerySpecificColumns(connection);
                        break;
                    case "5":
                        await QueryScalar(connection);
                        break;
                    case "b":
                        return;
                }
            }
        }

        static async Task QueryScalar(IDbConnection connection)
        {
            var sql = @"SELECT COUNT(*) FROM Product";

            var count = await connection.ExecuteScalarAsync<int>(sql);

            Console.WriteLine($"Product count: {count}");
        }

        static async Task QuerySingleRow(IDbConnection connection)
        {
            var sql = @"SELECT * FROM Product
                        WHERE Size = 38";

            var entity = await connection.QueryFirstOrDefaultAsync<Product>(sql);
            DisplayHelper.PrintJson(entity);

            var sql1 = @"SELECT * FROM Product
                        WHERE Id = 17";
            var entity1 = await connection.QuerySingleOrDefaultAsync<Product>(sql1);
            DisplayHelper.PrintJson(entity1);

            var sql2 = @"SELECT * FROM Product
                        WHERE Id = 200";

            var entity2 = await connection.QueryFirstAsync<Product>(sql2);
            DisplayHelper.PrintJson(entity2);
        }

        static async Task QueryMultipleRows(IDbConnection connection)
        {
            var sql = @"SELECT * FROM Product
                        WHERE Size = 41";

            var results = await connection.QueryAsync<Product>(sql);

            DisplayHelper.PrintJson(results);
        }

        static async Task QueryMultiResults(IDbConnection connection)
        {
            var sql =
                @"
                SELECT * FROM Product WHERE Id = 20;
                SELECT * FROM Brand";

            using var multi = await connection.QueryMultipleAsync(sql);
            {
                var entity = multi.ReadFirstOrDefault<Product>();
                DisplayHelper.PrintJson(entity);

                var results = multi.Read<Brand>();
                DisplayHelper.PrintJson(results);
            }
        }

        static async Task QuerySpecificColumns(IDbConnection connection)
        {
            var sql = @"SELECT Id, product_name, Size FROM Product 
                        WHERE Id = 20";

            var entity = await connection.QueryFirstOrDefaultAsync<Product>(sql);
            DisplayHelper.PrintJson(entity);

            var entity1 = await connection.QueryFirstOrDefaultAsync(sql);
            DisplayHelper.PrintJson(entity1);
        }
    }
}
