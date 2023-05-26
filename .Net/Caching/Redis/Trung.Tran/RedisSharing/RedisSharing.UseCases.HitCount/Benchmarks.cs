using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;
using RedisSharing.UseCases.HitCount.Helpers;
using RedisSharing.UseCases.HitCount.Models;
using RedisSharing.UseCases.HitCount.SqlServerStore;
using StackExchange.Redis;

namespace RedisSharing.UseCases.HitCount
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 500)]
    //[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 100)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
    public class Benchmarks
    {
        static ConnectionMultiplexer _redisMultiplexer;
        static readonly object _locker = new object();
        static readonly int _threadCount = 10;
        //static readonly int _threadCount = 100;

        static Benchmarks()
        {
            _redisMultiplexer = RedisHelper.GetConnectionMultiplexer();
        }

        public static async Task ResetAsync()
        {
            using (var context = new HitcountContext())
            {
                await context.Database.MigrateAsync();
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM HitCountItems");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM IpAddresses");

                var currentCount = await context.HitCountItems
                    .Where(i => i.Key == "normal_count")
                    .FirstOrDefaultAsync();

                if (currentCount == null)
                {
                    context.HitCountItems.Add(new HitCountItem
                    {
                        Key = "normal_count",
                        Count = 0
                    });

                    context.HitCountItems.Add(new HitCountItem
                    {
                        Key = "unique_count",
                        Count = 0
                    });

                    await context.SaveChangesAsync();
                }
            }

            var server = _redisMultiplexer.GetServer("localhost", 6379);
            await server.FlushDatabaseAsync();
        }

        [Benchmark, BenchmarkCategory("Normal count")]
        public async Task MSSQL_NormalCount()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new HitcountContext())
                {
                    var key = "normal_count";
                    await context.Database
                        .ExecuteSqlInterpolatedAsync($"UPDATE HitCountItems SET Count=Count+1 WHERE [Key] = {key}");
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Normal count")]
        public async Task Redis_NormalCount()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                await db.StringIncrementAsync("normal_count");
            });
        }

        static int MSSQL_Count = 0;
        [Benchmark, BenchmarkCategory("Unique count")]
        public async Task MSSQL_UniqueCount()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new HitcountContext())
                {
                    bool ipExists = false;

                    lock (_locker)
                    {
                        var ipAd = (++MSSQL_Count % 1000).ToString();

                        ipExists = context.IpAddresses
                            .Where(i => i.Ip == ipAd)
                            .Any();

                        if (!ipExists)
                        {
                            context.IpAddresses.Add(new IpAddress
                            {
                                Ip = ipAd,
                            });

                            context.SaveChanges();
                        }
                    }

                    if (!ipExists)
                    {
                        var key = "unique_count";
                        await context.Database
                            .ExecuteSqlInterpolatedAsync($"UPDATE HitCountItems SET Count=Count+1 WHERE [Key] = {key}");
                    }
                }
            });
        }

        static int Redis_Count = 0;
        [Benchmark(Baseline = true), BenchmarkCategory("Unique count")]
        public async Task Redis_UniqueCount()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var ipAd = (ThreadHelper.ThreadSafeIncrease(ref Redis_Count) % 1000).ToString();

                if (!await db.SetContainsAsync("ip_addresses", ipAd))
                {
                    await db.SetAddAsync("ip_addresses", ipAd);
                    await db.StringIncrementAsync("unique_count");
                }

                //await db.HyperLogLogAddAsync("hll_unique_count", ipAd);
            });
        }

        static int Hll_Redis_Count = 0;
        [Benchmark, BenchmarkCategory("Unique count")]
        public async Task Redis_UniqueCount_Hll()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var ipAd = (ThreadHelper.ThreadSafeIncrease(ref Hll_Redis_Count) % 1000).ToString();

                await db.HyperLogLogAddAsync("hll_unique_count", ipAd);
            });
        }
    }
}
