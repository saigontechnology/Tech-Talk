using BenchmarkDotNet.Attributes;
using Dapper;
using DapperSharing.Examples.Template;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Transactions;

namespace DapperSharing.Examples
{
    [MemoryDiagnoser]
    public class E10_DapperBenchmark : BenchmarkBase
    {

        [GlobalSetup]
        public void Setup()
        {
            BaseSetup();
        }

        [Benchmark]
        public Product Dapper_QueryFirstOrDefault()
        {
            return _connection.QueryFirstOrDefault<Product>("Select TOP 1 * from production.products Where ProductId = @ProductId", new { ProductId = 304 });
        }

        [Benchmark]
        public List<Product> Dapper_Filter()
        {
            return _connection.Query<Product>("Select * from production.products Where ModelYear = @ModelYear", new { ModelYear = 2018 }).ToList();
        }

        [Benchmark]
        public Product Dapper_Add_Delete()
        {
            var product = new Product()
            {
                ProductName = "test - 2018",
                ModelYear = 2018,
                BrandId = 9,
                CategoryId = 6,
                ListPrice = 392
            };

            var id = _connection.ExecuteScalar<int>("INSERT INTO production.products (ProductName, ModelYear, BrandId, CategoryId, ListPrice)" +
                "OUTPUT INSERTED.[ProductId]" +
                "VALUES (@ProductName, @ModelYear, @BrandId, @CategoryId, @ListPrice);", product);

            _connection.Execute("DELETE FROM production.products WHERE ProductId = @ProductId", new { ProductId = id });

            return product;
        }

    }
}
