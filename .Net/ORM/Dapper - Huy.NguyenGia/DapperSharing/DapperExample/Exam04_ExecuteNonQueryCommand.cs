using Dapper;
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
    public static class Exam04_ExecuteNonQueryCommand
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E04_ExecuteNonQueryCommand ===========");
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
                INSERT INTO Product
                    (product_name, Quantity, Size, IdProductDetail)
                VALUES 
                    (@ProductName, @Quantity, @Size, @IdProductDetail);";

            var count = await connection.ExecuteAsync(sql, new Product
            {
                ProductName = "NIKE 2023",
                Quantity = 5,
                Size = 42,
                IdProductDetail = 65
            });
            Console.WriteLine($"Affected rows: {count}");
        }
        static async Task Update(IDbConnection connection)
        {
            var sql = @"UPDATE Product 
                        SET ProductName = @ProductName
                        WHERE Id = @ProductId;";

            var count = await connection.ExecuteAsync(sql, new
            {
                ProductId = 20,
                ProductName = "My new product 2023 has been updated"
            });
            Console.WriteLine($"Affected rows: {count}");
        }

        static async Task MultipleCommands(IDbConnection connection)
        {
            var sql = @"UPDATE Product 
                        SET IsDeleted = 1
                        WHERE Id = @Id;

                        DELETE FROM Product
                        WHERE ProductName = @Name;";

            var count = await connection.ExecuteAsync(sql, new
            {
                Id = 20,
                Name = "NIKE 2023"
            });
            Console.WriteLine($"Affected rows: {count}");
        }


    }
}
