using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedisSharing.UseCases.Caching.Helpers;
using RedisSharing.UseCases.Caching.Models;
using RedisSharing.UseCases.Caching.SqlServerStore;
using StackExchange.Redis;

namespace RedisSharing.UseCases.Caching
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 14)]
    //[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 7)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
    public class Benchmarks
    {
        static ConnectionMultiplexer _redisMultiplexer;
        static readonly int _threadCount = 10;
        //static readonly int _threadCount = 100;
        static readonly object _lock = new object();

        static Benchmarks()
        {
            _redisMultiplexer = RedisHelper.GetConnectionMultiplexer();
        }

        public static async Task ResetAsync()
        {
            var fileText = File.ReadAllText("Resources/countries.json");
            var allCountries = JsonConvert.DeserializeObject<CountryObject>(fileText);
            var countryEntities = allCountries.Countries
                .Select(c => new CountryItem
                {
                    Name = c.Country
                })
                .DistinctBy(o => o.Name)
                .ToArray();

            var states = allCountries.Countries
                .SelectMany(c => c.States.Select(s => new CountryState
                {
                    Name = s,
                    CountryName = c.Country
                })
                .DistinctBy(s => s.Name)
                .ToArray());

            using (var context = new CachingContext())
            {
                await context.Database.MigrateAsync();
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM States");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Countries");

                context.AddRange(countryEntities);
                await context.SaveChangesAsync();
                context.AddRange(states);
                await context.SaveChangesAsync();
            }

            var server = _redisMultiplexer.GetServer("localhost", 6379);
            await server.FlushDatabaseAsync();
        }

        [Benchmark, BenchmarkCategory("Read all")]
        public async Task MSSQL_ReadAll()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                using (var context = new CachingContext())
                {
                    var allCountries = await context.Countries
                        .Select(o => new
                        {
                            o.Name,
                            o.States
                        }).ToArrayAsync();
                }
            });
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Read all")]
        public async Task Redis_ReadAll()
        {
            await ThreadHelper.RunMultipleAsync(_threadCount, async () =>
            {
                var db = _redisMultiplexer.GetDatabase();
                var cKey = "countries";

                var exists = await db.KeyExistsAsync(cKey);

                if (!exists)
                {
                    lock (_lock)
                    {
                        if (!db.KeyExists(cKey))
                        {
                            using (var context = new CachingContext())
                            {
                                var allCountriesInit = context.Countries
                                    .Select(o => new CountryModel
                                    {
                                        Country = o.Name,
                                        States = o.States.Select(s => s.Name)
                                    }).ToArray();

                                db.StringSet(cKey,
                                    JsonConvert.SerializeObject(allCountriesInit),
                                    expiry: TimeSpan.FromSeconds(60));

                                return;
                            }
                        }
                    }
                }

                var allCountriesStr = await db.StringGetAsync(cKey);
                var allCountries = JsonConvert.DeserializeObject<IEnumerable<CountryModel>>(allCountriesStr);
            });
        }
    }
}
