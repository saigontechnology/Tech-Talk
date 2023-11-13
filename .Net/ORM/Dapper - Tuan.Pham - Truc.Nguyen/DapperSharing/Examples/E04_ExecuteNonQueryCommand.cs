using Dapper;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperSharing.Examples
{
    public static class E04_ExecuteNonQueryCommand
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E04_ExecuteNonQueryCommand ===========");
            DisplayHelper.PrintListOfMethods(typeof(E04_ExecuteNonQueryCommand));
            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        await Insert(connection);
                        break;
                    case "2":
                        await Update(connection);
                        break;
                    case "3":
                        await MultipleCommands(connection);
                        break;
                }
            }
        }

        static async Task Insert(IDbConnection connection)
        {
            var sql = @"
                INSERT INTO production.products
                    (ProductName, BrandId, CategoryId, ModelYear, ListPrice)
                VALUES 
                    (@ProductName, @BrandId, @CategoryId, @ModelYear, @ListPrice);";
                
            var count = await connection.ExecuteAsync(sql, new Product
            {
                BrandId = 1,
                CategoryId = 1,
                ListPrice = 150,
                ModelYear = 2023,
                ProductName = "My 2023 Product"
            });
            Console.WriteLine($"Affected rows: {count}");
        }

        static async Task Update(IDbConnection connection)
        {
            var sql = @"UPDATE production.products 
                        SET ProductName = @ProductName
                        WHERE ProductId = @ProductId;";

            var count = await connection.ExecuteAsync(sql, new
            {
                ProductId = 336,
                ProductName = "My new product 2023 has been updated"
            });
            Console.WriteLine($"Affected rows: {count}");
        }

        static async Task MultipleCommands(IDbConnection connection)
        {
            var sql = @"UPDATE production.products 
                        SET ProductName = ''
                        WHERE ModelYear = @ModelYear;

                        DELETE FROM production.products
                        WHERE ProductId = @Id;";

            var count = await connection.ExecuteAsync(sql, new
            {
                ModelYear = 2023,
                Id = 336
            });
            Console.WriteLine($"Affected rows: {count}");
        }
    }
}
