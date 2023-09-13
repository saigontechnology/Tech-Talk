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
    public static class Exam08_Transaction
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E08_Others ===========");
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        await TransactionCommit(connection);
                        break;
                    //case "2":
                    //    await TransactionRollback(connection);
                    //    break;
                    //case "3":
                    //    await TransactionScopeRollBack(connection);
                    //    break;
                    //case "4":
                    //    await TempTable(connection);
                    //    break;
                }
            }
        }

        static async Task TransactionCommit(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @$"DELETE FROM Product WHERE Id = @Id";

                await connection.ExecuteAsync(sql, new
                {
                    Id = 30
                }, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                transaction.Rollback();
            }
        }

        static void Unbuffered(IDbConnection connection)
        {
            var sql = "SELECT * FROM Product";

            var products = connection.Query<Product>(sql, buffered: false);

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id} {product.ProductName}");
            }
        }

        static async Task BufferedAsync(IDbConnection connection)
        {
            var sql = "SELECT * FROM Product";

            var products = await connection.QueryAsync<Product>(sql);

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id} {product.ProductName}");
            }
        }

    }
}
