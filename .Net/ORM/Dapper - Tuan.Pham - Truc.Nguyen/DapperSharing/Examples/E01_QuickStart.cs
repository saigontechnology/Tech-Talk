using Dapper;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;

namespace DapperSharing.Examples
{
    public static class E01_QuickStart
    {
        public static void Run()
        {
            Console.WriteLine($"=========== RUNNING {nameof(E01_QuickStart)} ===========");

            IEnumerable<Product> results;
            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var sql = @"
SELECT * FROM production.products 
ORDER BY ProductId
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY";

                results = connection.Query<Product>(sql);
            }

            DisplayHelper.PrintJson(results);
        }
    }
}
