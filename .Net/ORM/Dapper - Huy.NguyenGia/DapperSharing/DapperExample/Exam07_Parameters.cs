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
    public static class Exam07_Parameters
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E07_Parameters ===========");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        await SqlInjection(connection);
                        break;
                    case "2":
                        await AnonymousParameters(connection);
                        break;
                    case "3":
                        await DynamicParameters(connection);
                        break;
                    case "4":
                        await StringParameters(connection);
                        break;
                    //case "5":
                    //    await LiteralReplacements(connection);
                    //    break;
                    case "6":
                        await WhereInParameters(connection);
                        break;
                    case "7":
                        await OutputParameters(connection);
                        break;
                    case "8":
                        await TableValuedParameters(connection);
                        break;
                    case "9":
                        await ParameterSniffing(connection);
                        break;
                }
            }
        }

        static async Task SqlInjection(IDbConnection connection)
        {
            try
            {
                Console.Write("Search products: ");
                var search = Console.ReadLine();

                var sql = @$"SELECT * FROM Product WHERE product_name LIKE '%{search}%'";

                var products = await connection.QueryAsync<Product>(sql);

                DisplayHelper.PrintJson(products);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        static async Task AnonymousParameters(IDbConnection connection)
        {
            var search = "hello; SELECT * FROM Customer;";

            var sql = @"
                    SELECT * FROM Product 
                    WHERE product_name LIKE @Search OR Size = @Size";

            var products = await connection.QueryAsync<Product>(sql, new
            {
                Search = $"%{search}%",
                Size = 40
            });

            DisplayHelper.PrintJson(products);
        }

        static async Task DynamicParameters(IDbConnection connection)
        {
            var dynamicParameters = new DynamicParameters(new { ProductId = 20 });

            dynamicParameters.AddDynamicParams(new { NameContains = "%adidas%", ProductDetailId = 2 });

            dynamicParameters.Add("@NameEquals", "nike", DbType.String, ParameterDirection.Input, 10);

            var sql = @"
                        SELECT * FROM Product 
                        WHERE product_name LIKE @NameContains
                        OR product_name = @NameEquals
                        OR IdProductDetail = @ProductDetailId;
                        ";

            var products = await connection.QueryAsync<Product>(sql, dynamicParameters);

            DisplayHelper.PrintJson(products);
        }

        static async Task StringParameters(IDbConnection connection)
        {
            string sql = @"SELECT * FROM Product WHERE product_name LIKE @Name";

            var dbParams = new DbString()
            {
                Value = "%adidas%", 
                IsAnsi = true,  // 'n(var)char' or '(var)char'
                IsFixedLength = false, // 'char' or 'varchar'
                Length = 7
            };

            var firstProduct = await connection.QueryFirstOrDefaultAsync<Product>(sql,
                new
                {
                    Name = dbParams
                });

            DisplayHelper.PrintJson(firstProduct);
        }

        static async Task WhereInParameters(IDbConnection connection)
        {
            string sql = @"SELECT * FROM Product WHERE Id IN @Ids";

            var products = await connection.QueryAsync<Product>(sql,
                new
                {
                    Ids = new[] { 11, 12, 14, 15 }
                });

            DisplayHelper.PrintJson(products);
        }

        static async Task OutputParameters(IDbConnection connection)
        {
            const string ProcName = "GetProductDetails";
            string createProcSql = @$"
CREATE OR ALTER PROC {ProcName}
   @ProductId          INT,
   @Name               NVARCHAR(Max)         OUTPUT,
   @Size          INT                   OUTPUT
AS
   SELECT
      @Name = product_name,
      @Size = Size FROM Product
   WHERE Id=@ProductId
";

            await connection.ExecuteAsync(createProcSql);

            var parameters = new DynamicParameters(new
            {
                ProductId = 11
            });
            parameters.Add("@Name", null, dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
            parameters.Add("@Size", null, dbType: DbType.Int16, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(ProcName, parameters, commandType: CommandType.StoredProcedure);

            var name = parameters.Get<string>("@Name");
            var size = parameters.Get<short>("@Size");

            DisplayHelper.PrintJson($"{name} - {size}");
        }

        static async Task TableValuedParameters(IDbConnection connection)
        {
            var tvpExampleType = new DataTable();
            tvpExampleType.Columns.Add("Id", typeof(int));
            tvpExampleType.Columns.Add("Name", typeof(string));

            tvpExampleType.Rows.Add(1, "Jake");
            tvpExampleType.Rows.Add(2, "Benny");

            var p = new DynamicParameters();
            p.Add("TvpExampleType", tvpExampleType.AsTableValuedParameter("dbo.TvpExampleType"));
            await connection.ExecuteAsync("dbo.MyStoredProc", p, commandType: CommandType.StoredProcedure);
        }

        static async Task ParameterSniffing(IDbConnection connection)
        {

            var sql = @"
                     SELECT * FROM Product 
                     WHERE Size >= @Size";

            var products = await connection.QueryAsync<Product>(sql, new
            {
                Size = 36
            });

            DisplayHelper.PrintJson(products);
        }
    }
}
