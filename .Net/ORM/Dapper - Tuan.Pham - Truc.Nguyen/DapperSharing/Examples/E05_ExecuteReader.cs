using Dapper;
using DapperSharing.Models;
using Microsoft.Data.SqlClient;
using Perfolizer.Mathematics.SignificanceTesting;
using System.Data;

namespace DapperSharing.Examples
{
    public static class E05_ExecuteReader
    {
        public static async Task Run()
        {
            Console.WriteLine("=========== RUNNING E05_ExecuteReader ===========");
            using (var connection = new SqlConnection(Program.DBInfo.ConnectionString))
            {
                await QueryProducts(connection);
            }
        }

        static async Task QueryProducts(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products";

            var dataReader = await connection.ExecuteReaderAsync(sql);
            var products = new List<Product>();

            DataTable datatable = new();
            datatable.Load(dataReader);

            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                Product product = new()
                {
                    ProductId = Convert.ToInt32(datatable.Rows[i]["ProductId"]),
                    ProductName = datatable.Rows[i]["ProductName"].ToString(),
                    ModelYear = Convert.ToInt16(datatable.Rows[i]["ModelYear"]),
                    ListPrice = Convert.ToDecimal(datatable.Rows[i]["ListPrice"])
                };
                products.Add(product);
            }

            while (dataReader.Read())
            {
                Product product = new()
                {
                    ProductId = dataReader.GetInt32(0),
                    ProductName = dataReader.GetString(1),
                    ModelYear = dataReader.GetInt16(5),
                    ListPrice = dataReader.GetDecimal(6),
                };
                products.Add(product);
            }
        }
    }
}
