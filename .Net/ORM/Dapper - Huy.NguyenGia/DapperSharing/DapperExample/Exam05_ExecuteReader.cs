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
    public static class Exam05_ExecuteReader
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
            var sql = @"SELECT * FROM Product";

            var dataReader = await connection.ExecuteReaderAsync(sql);
            var products = new List<Product>();

            //DataTable datatable = new();
            //datatable.Load(dataReader);

            //for (int i = 0; i < datatable.Rows.Count; i++)
            //{
            //    Product product = new()
            //    {
            //        Id = Convert.ToInt32(datatable.Rows[i]["Id"]),
            //        ProductName = datatable.Rows[i]["product_name"].ToString(),
            //        Quantity = Convert.ToInt16(datatable.Rows[i]["Quantity"]),
            //        Size = Convert.ToInt16(datatable.Rows[i]["Size"]),
            //    };
            //    products.Add(product);
            //}

            while (dataReader.Read())
            {
                Product product = new()
                {
                    Id = dataReader.GetInt32(0),
                    ProductName = dataReader.GetString(1),
                    Quantity = dataReader.GetInt32(2),
                    Size = dataReader.GetInt32(3),
                };
                products.Add(product);
            }
            DisplayHelper.PrintJson(products);
        }
    }
}
