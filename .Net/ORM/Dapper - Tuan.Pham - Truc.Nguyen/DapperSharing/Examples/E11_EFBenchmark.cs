using BenchmarkDotNet.Attributes;
using Dapper;
using DapperSharing.Examples.Template;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

namespace DapperSharing.Examples
{
    [MemoryDiagnoser]
    public class E11_EFBenchmark : BenchmarkBase
    {
        private BikeStoresContext _dbContext;
        private static readonly Func<BikeStoresContext, int, Product> compiledQuery =
            EF.CompileQuery((BikeStoresContext ctx, int id) => ctx.Products.First(p => p.ProductId == id));

        [GlobalSetup]
        public void Setup()
        {
            BaseSetup();
            _dbContext = new BikeStoresContext(Program.DBInfo.ConnectionString);
        }

        [Benchmark]
        public Product EFCore_FirstOrDefault()
        {
            return _dbContext.Products.FirstOrDefault(x => x.ProductId == 304);
        }
        [Benchmark]
        public List<Product> EFCore_Filter()
        {
            return _dbContext.Products.Where(x => x.ModelYear == 2018).ToList();
        }
        [Benchmark]
        public Product EFCore_Add_Delete()
        {
            var product = new Product()
            {
                ProductName = "test - 2018",
                ModelYear = 2018,
                BrandId = 9,
                CategoryId = 6,
                ListPrice = 392
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return product;
        }

    }
}
