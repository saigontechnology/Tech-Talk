using Dapper;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace DapperSharing.Examples
{
    public static class E08_Others
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E08_Others ===========");
            DisplayHelper.PrintListOfMethods(typeof(E08_Others));
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        await TransactionCommit(connection);
                        break;
                    case "2":
                        await TransactionRollback(connection);
                        break;
                    case "3":
                        await TransactionScopeRollBack(connection);
                        break;
                    case "4":
                        await TempTable(connection);
                        break;
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
                var sql = @$"DELETE FROM production.products WHERE ProductName = @Name";

                await connection.ExecuteAsync(sql, new
                {
                    Name = "TransactionTest"
                }, transaction: transaction);

                //await connection.ExecuteAsync(sql, new
                //{
                //    Id = 1000
                //}, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                transaction.Rollback();
            }
        }

        static async Task TransactionRollback(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @$"DELETE FROM production.products WHERE ProductName = @Name";

                await connection.ExecuteAsync(sql, new
                {
                    Name = "TransactionTest"
                }, transaction: transaction);

                await connection.ExecuteAsync(sql, new
                {
                    Id = 1000
                }, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                transaction.Rollback();
            }
        }

        static async Task TransactionScopeRollBack(IDbConnection connection)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var sql = @$"DELETE FROM production.products WHERE ProductName = @Name";

                    await connection.ExecuteAsync(sql, new
                    {
                        Name = "TransactionTest"
                    });

                    await connection.ExecuteAsync(sql, new
                    {
                        Id = 1000
                    });

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
        }

        static async Task<IEnumerable<int>> TempTable(IDbConnection connection)
        {
            connection.Open();

            await connection.ExecuteAsync(@"CREATE TABLE #tmpOrder(orderId int);");
            await connection.ExecuteAsync(@"INSERT INTO #tmpOrder(orderId) VALUES (1);");

            return await connection.QueryAsync<int>(@"SELECT * FROM #tmpOrder;");
        }


        static void Buffered(IDbConnection connection)
        {
           var sql = "Select * from production.products";

           var products = connection.Query<Product>(sql);

           foreach (var product in products)
           {
               Console.WriteLine($"{product.ProductId} {product.ProductName}");
           }
        }

        static void Unbuffered(IDbConnection connection)
        {
           var sql = "Select * from production.products";

           var customers = connection.Query<Customer>(sql, buffered: false);

           foreach (var customer in customers)
           {
               Console.WriteLine($"{customer.CustomerId} {customer.FirstName}");
           }
        }

        static async Task BufferedAsync(IDbConnection connection)
        {
           var sql = "Select * from production.products";

           var customers = await connection.QueryAsync<Customer>(sql);

           foreach (var customer in customers)
           {
               Console.WriteLine($"{customer.CustomerId} {customer.FirstName}");
                }
        }

        static async Task UnbufferedAsync(DbConnection connection)
        {
           var sql = "Select * from production.products";

           var customers = connection.QueryUnbufferedAsync<Customer>(sql);

           await foreach (var customer in customers)
           {
               Console.WriteLine($"{customer.CustomerId} {customer.FirstName}");
           }
        }
    }
}
