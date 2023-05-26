using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedisSharing.UseCases.ShoppingCart.Helpers;
using RedisSharing.UseCases.ShoppingCart.Models;
using RedisSharing.UseCases.ShoppingCart.SqlServerStore;
using StackExchange.Redis;

namespace RedisSharing.UseCases.ShoppingCart
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 500)]
    //[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 100)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
    public class Benchmarks
    {
        static ConnectionMultiplexer _redisMultiplexer;
        static readonly int _threadCount = 10;
        //static readonly int _threadCount = 100;

        static Benchmarks()
        {
            _redisMultiplexer = RedisHelper.GetConnectionMultiplexer();
        }

        public static async Task ResetAsync()
        {
            using (var context = new ShoppingCartContext())
            {
                await context.Database.MigrateAsync();
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM ShoppingCartItems");

                if (!context.AppUsers.Any())
                {
                    for (var i = 0; i < 30000; i++)
                    {
                        context.AppUsers.Add(new AppUser
                        {
                            Name = i.ToString()
                        });

                        context.Products.Add(new Product
                        {
                            Name = i.ToString(),
                        });
                    }

                    await context.SaveChangesAsync();
                }
            }

            var server = _redisMultiplexer.GetServer("localhost", 6379);
            await server.FlushDatabaseAsync();
        }

        static int MSSQL_InsertItem_ProdId = 0;
        [Benchmark, BenchmarkCategory("Insert")]
        public async Task MSSQL_InsertItem()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ShoppingCartContext())
                {
                    context.ShoppingCartItems.Add(new ShoppingCartItem
                    {
                        AddedTime = DateTimeOffset.UtcNow,
                        ProductName = ThreadHelper.ThreadSafeIncrease(ref MSSQL_InsertItem_ProdId).ToString(),
                        UserName = "1",
                    });

                    await context.SaveChangesAsync();
                }
            });
        }

        [Benchmark, BenchmarkCategory("List")]
        public async Task MSSQL_GetItems()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ShoppingCartContext())
                {
                    var allItems = await context.ShoppingCartItems
                        .AsNoTracking()
                        .Take(10)
                        .Select(o => new ShoppingCartItem
                        {
                            UserName = o.UserName,
                            AddedTime = DateTime.Now,
                            Id = o.Id,
                            ProductName = o.ProductName
                        })
                        .ToArrayAsync();
                }
            });
        }

        static int MSSQL_RemoveItem_ProdId = 0;
        [Benchmark, BenchmarkCategory("Remove")]
        public async Task MSSQL_RemoveItem()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ShoppingCartContext())
                {
                    var currentProdName = ThreadHelper.ThreadSafeIncrease(ref MSSQL_RemoveItem_ProdId).ToString();

                    var firstItem = await context.ShoppingCartItems
                        .Where(o => o.UserName == "1" && o.ProductName == currentProdName)
                        .Select(o => new ShoppingCartItem
                        {
                            Id = o.Id
                        })
                        .FirstOrDefaultAsync();

                    if (firstItem != null)
                    {
                        context.Entry(firstItem).State = EntityState.Deleted;
                        await context.SaveChangesAsync();
                    }
                }
            });
        }

        static int Redis_InsertItem_ProdId = 0;
        [Benchmark(Baseline = true), BenchmarkCategory("Insert")]
        public async Task Redis_InsertItem()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var item = new ShoppingCartItem
                {
                    AddedTime = DateTimeOffset.UtcNow,
                    ProductName = ThreadHelper.ThreadSafeIncrease(ref Redis_InsertItem_ProdId).ToString(),
                    UserName = "1",
                };

                var json = JsonConvert.SerializeObject(item);

                await db.HashSetAsync($"user_{item.UserName}_cart_items", item.ProductName, json);
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("List")]
        public async Task Redis_GetItems()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var userName = "1";

                var allItems = (await db.HashGetAllAsync(userName))
                    //.Take(10)
                    .Select(o => JsonConvert.DeserializeObject<ShoppingCartItem>(o.Value))
                    .ToArray();
            });
        }

        static int Redis_RemoveItem_ProdId = 0;
        [Benchmark(Baseline = true), BenchmarkCategory("Remove")]
        public async Task Redis_RemoveItem()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var userName = "1";
                var prodName = ThreadHelper.ThreadSafeIncrease(ref Redis_RemoveItem_ProdId).ToString();
                var hKey = $"user_{userName}_cart_items";

                var exists = await db.HashExistsAsync(hKey, prodName);

                if (exists)
                {
                    await db.HashDeleteAsync(hKey, prodName);
                }
            });
        }
    }
}
