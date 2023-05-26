using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;
using RedisSharing.UseCases.CQRS.Helpers;
using RedisSharing.UseCases.CQRS.Models;
using RedisSharing.UseCases.CQRS.SqlServerStore;
using StackExchange.Redis;

namespace RedisSharing.UseCases.CQRS
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 100)]
    //[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 10)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
    public class Benchmarks
    {
        static ConnectionMultiplexer _redisMultiplexer;
        static readonly int _threadCount = 10;
        //static readonly int _threadCount = 100;
        static readonly int SampleMonth = 2;
        static readonly int SampleYear = 2022;

        static Benchmarks()
        {
            _redisMultiplexer = RedisHelper.GetConnectionMultiplexer();
        }

        public static async Task ResetAsync()
        {
            var server = _redisMultiplexer.GetServer("localhost", 6379);
            await server.FlushDatabaseAsync();

            using (var context = new OrderingContext())
            {
                await context.Database.MigrateAsync();
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM OrderItems");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Orders");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Products");

                for (var i = 0; i < 100; i++)
                {
                    context.Products.Add(new Product
                    {
                        Id = i.ToString(),
                        Name = i.ToString(),
                        Price = i * 100
                    });
                }

                await context.SaveChangesAsync();

                var orders = new List<Models.Order>();

                for (var i = 0; i < 10000; i++)
                {
                    var orderId = (i % 1000).ToString();
                    var customerName = (i % new Random().Next(10, 50)).ToString();
                    var prodId = (i % 100).ToString();
                    var order = orders.FirstOrDefault(o => o.Id == orderId);
                    if (order == null)
                    {
                        order = new Models.Order();
                        order.Id = orderId;
                        order.OrderedTime = new DateTimeOffset(SampleYear, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i / 2);
                        order.CustomerName = customerName;
                        order.Items = new List<Models.OrderItem>();
                        orders.Add(order);
                    }
                    order.Items.Add(new Models.OrderItem
                    {
                        Id = i.ToString(),
                        ProductId = prodId,
                        Quantity = i + 1,
                        UnitPrice = i * 100,
                        Unit = "unit"
                    });
                }

                foreach (var order in orders)
                {
                    context.Add(order);
                    OnCreateOrder(_redisMultiplexer, order);
                }

                await context.SaveChangesAsync();
            }
        }

        [Benchmark, BenchmarkCategory("Monthly Report")]
        public async Task MSSQL_MonthlyReportFromQuery()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new OrderingContext())
                {
                    // [Important] Order statistic per month
                    var orderPerMonth = await context
                        .OrderItems
                        .Where(o => o.Order.OrderedTime.Month == SampleMonth)
                        .Where(o => o.Order.OrderedTime.Year == SampleYear)
                        .GroupBy(o => o.OrderId)
                        .Select(group => new
                        {
                            TotalAmount = group.Sum(i => i.Quantity * i.UnitPrice)
                        }).ToArrayAsync();
                    var totalQuantityPerMonth = orderPerMonth.Count();
                    var totalAmountPerMonth = orderPerMonth.Sum(o => o.TotalAmount);

                    // [Important] Order statistic per month per customer
                    var orderQuantityPerMonthPerCustomer = await context
                        .Orders
                        .Where(o => o.OrderedTime.Month == SampleMonth)
                        .Where(o => o.OrderedTime.Year == SampleYear)
                        .GroupBy(o => o.CustomerName)
                        .Select(group => new
                        {
                            Customer = group.Key,
                            TotalQuantity = group.Count(),
                        }).ToArrayAsync();
                    var orderAmountPerMonthPerCustomer = await context
                        .OrderItems
                        .Where(o => o.Order.OrderedTime.Month == SampleMonth)
                        .Where(o => o.Order.OrderedTime.Year == SampleYear)
                        .GroupBy(o => o.Order.CustomerName)
                        .Select(group => new
                        {
                            Customer = group.Key,
                            TotalAmount = group.Sum(o => o.Quantity * o.UnitPrice),
                        }).ToArrayAsync();
                    var opmpc =
                        (from oQuantity in orderQuantityPerMonthPerCustomer
                         join oAmount in orderAmountPerMonthPerCustomer
                         on oQuantity.Customer equals oAmount.Customer
                         select new
                         {
                             oQuantity.Customer,
                             oQuantity.TotalQuantity,
                             oAmount.TotalAmount
                         }).ToArray();

                    // [Important] Order statistic per month per product
                    var orderAmountPerMonthPerProd = await context
                        .OrderItems
                        .Where(o => o.Order.OrderedTime.Month == SampleMonth)
                        .Where(o => o.Order.OrderedTime.Year == SampleYear)
                        .GroupBy(o => o.ProductId)
                        .Select(group => new
                        {
                            Product = group.Key,
                            TotalAmount = group.Sum(o => o.Quantity * o.UnitPrice),
                        }).ToArrayAsync();
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Monthly Report")]
        public async Task Redis_MonthlyReportFromStorage()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var opmKey = $"order-per-month-{SampleMonth}-{SampleYear}";
                var report = await db.HashGetAllAsync(opmKey);

                //do stuff;
            });
        }

        [Benchmark, BenchmarkCategory("Year Report")]
        public async Task MSSQL_YearReportFromQuery()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new OrderingContext())
                {
                    // [Important] Order statistic per year
                    var orderPerYear = await context
                        .OrderItems
                        .Where(o => o.Order.OrderedTime.Year == SampleYear)
                        .GroupBy(o => o.OrderId)
                        .Select(group => new
                        {
                            TotalAmount = group.Sum(i => i.Quantity * i.UnitPrice)
                        }).ToArrayAsync();
                    var totalQuantityPerYear = orderPerYear.Count();
                    var totalAmountPerYear = orderPerYear.Sum(o => o.TotalAmount);
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Year Report")]
        public async Task Redis_YearReportFromStorage()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var opyKey = $"order-per-year-{SampleYear}";
                var report = await db.HashGetAllAsync(opyKey);

                //do stuff;
            });
        }

        /// <summary>
        /// [Important] simulate order creted event handler
        /// Can use async/await, threading to improve performance
        /// Can be replaced, discarded and rebuilt from scratch 
        /// </summary>
        private static void OnCreateOrder(
            ConnectionMultiplexer connectionMultiplexer,
            Models.Order order)
        {
            var db = connectionMultiplexer.GetDatabase();
            var batch = db.CreateBatch();
            var orderAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            // [Important] Order statistic per year
            var opyKey = $"order-per-year-{order.OrderedTime.Year}";
            Task _ = batch.HashIncrementAsync(opyKey, "total_quantity", 1);
            _ = batch.HashIncrementAsync(opyKey, "total_amount", orderAmount);

            // [Important] Order statistic per month
            var opmKey = $"order-per-month-{order.OrderedTime.Month}-{order.OrderedTime.Year}";
            _ = batch.HashIncrementAsync(opmKey, "total_quantity", 1);
            _ = batch.HashIncrementAsync(opmKey, "total_amount", orderAmount);

            // [Important] Order statistic per month per customer
            _ = batch.HashIncrementAsync(opmKey, $"total_quantity_customer_{order.CustomerName}", 1);
            _ = batch.HashIncrementAsync(opmKey, $"total_amount_customer_{order.CustomerName}", orderAmount);

            // [Important] Order statistic per month per product
            foreach (var item in order.Items)
            {
                _ = batch.HashIncrementAsync(opmKey, $"total_quantity_product_{item.ProductId}", item.Quantity);
                _ = batch.HashIncrementAsync(opmKey, $"total_amount_product_{item.ProductId}", item.Quantity * item.UnitPrice);
            }

            batch.Execute();
        }
    }
}
