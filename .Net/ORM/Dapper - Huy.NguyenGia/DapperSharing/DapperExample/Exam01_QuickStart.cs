using Dapper;
using DapperSharing.Helper;
using DapperSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSharing.DapperExample
{
    public static class Exam01_QuickStart
    {
        public static void Run()
        {
            Console.WriteLine($"=========== RUNNING {nameof(Exam01_QuickStart)} ===========");
            IEnumerable<Product> results;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var sql = @"
                            SELECT * FROM Product 
                            ORDER BY Id
                            OFFSET 0 ROWS
                            FETCH NEXT 10 ROWS ONLY";

                results = connection.Query<Product>(sql);
            }
            DisplayHelper.PrintJson(results);
        }
    }
}
