using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedisSharing.UseCases.ScoringTable.Helpers;
using RedisSharing.UseCases.ScoringTable.Models;
using RedisSharing.UseCases.ScoringTable.SqlServerStore;
using StackExchange.Redis;

namespace RedisSharing.UseCases.ScoringTable
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
            using (var context = new ScoringContext())
            {
                await context.Database.MigrateAsync();
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM PlayerRankRecords");
            }

            var server = _redisMultiplexer.GetServer("localhost", 6379);
            await server.FlushDatabaseAsync();
        }

        [Benchmark, BenchmarkCategory("Add record")]
        public async Task MSSQL_AddRecord()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ScoringContext())
                {
                    context.PlayerRankRecords.Add(new PlayerRankRecord
                    {
                        PlayerName = Guid.NewGuid().ToString(),
                        RecordedTime = DateTime.UtcNow,
                        Score = new Random().Next(10000)
                    });

                    await context.SaveChangesAsync();
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Add record")]
        public async Task Redis_AddRecord()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var record = new PlayerRankRecord
                {
                    PlayerName = Guid.NewGuid().ToString(),
                    RecordedTime = DateTime.UtcNow,
                    Score = new Random().Next(10000)
                };

                var serialized = JsonConvert.SerializeObject(record);
                var batch = db.CreateBatch();
                Task _ = batch.SortedSetAddAsync("scoring_by_score", serialized, record.Score);
                _ = batch.SortedSetAddAsync("scoring_by_time", serialized, record.RecordedTime.Ticks);
                batch.Execute();

                return Task.CompletedTask;
            });
        }

        [Benchmark, BenchmarkCategory("Top 10 Highest score")]
        public async Task MSSQL_HighestScore()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ScoringContext())
                {
                    var top10 = await context.PlayerRankRecords
                        .OrderByDescending(o => o.Score)
                        .Take(10)
                        .ToArrayAsync();
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Top 10 Highest score")]
        public async Task Redis_HighestScore()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var top10 = await db.SortedSetRangeByRankAsync("scoring_by_score", 0, 9, Order.Descending);
            });
        }

        [Benchmark, BenchmarkCategory("Top 10 Latest play")]
        public async Task MSSQL_LatestPlay()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new ScoringContext())
                {
                    var top10 = await context.PlayerRankRecords
                        .OrderByDescending(o => o.RecordedTime)
                        .Take(10)
                        .ToArrayAsync();
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Top 10 Latest play")]
        public async Task Redis_LatestPlay()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();

                var top10 = await db.SortedSetRangeByRankAsync("scoring_by_time", 0, 9, Order.Descending);
            });
        }

    }
}
