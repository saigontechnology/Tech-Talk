using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisSharing.Topics
{
    public static class DataTypeTopic
    {
        public static async Task StartAsync()
        {
            using var redis = Program.GetConnectionMultiplexer();
            var db = redis.GetDatabase();
            var server = redis.GetServer("localhost", 6379);

            var allKeys = server.Keys().Select(k => k.ToString());
            Console.WriteLine("Current keys: {0}", JsonConvert.SerializeObject(allKeys));

            await server.FlushDatabaseAsync();

            await db.StringSetAsync("string_key", "string_value", expiry: TimeSpan.FromMinutes(1));
            var strVal = await db.StringGetAsync("string_key");
            Console.WriteLine("String value: {0}", strVal);

            await db.HashSetAsync("person", new[]
            {
                new HashEntry("name", "Anonymous"),
                new HashEntry("age", "22"),
            });
            var person = await db.HashGetAllAsync("person");
            Console.WriteLine("Hash value: {0}", JsonConvert.SerializeObject(person.ToDictionary()));

            await db.ListRightPushAsync("list_key", new RedisValue[] { 1, 2, 3, 4, 5, 6, "ABC" });
            var listVal = await db.ListRangeAsync("list_key");
            Console.WriteLine("List value: {0}", JsonConvert.SerializeObject(listVal));

            await db.SetAddAsync("set_key", new RedisValue[] { 1, 1, 2, 2, 3, 3, "ABC" });
            var setVal = await db.SetMembersAsync("set_key");
            Console.WriteLine("Set value: {0}", JsonConvert.SerializeObject(setVal));

            await db.SortedSetAddAsync("sorted_set_key", new[]
            {
                new SortedSetEntry("TNT", 1),
                new SortedSetEntry("ABC", 2),
                new SortedSetEntry("XYZ", 3),
                new SortedSetEntry("ABC", 4),
            });
            var sortedSet = await db.SortedSetRangeByRankAsync("sorted_set_key");
            Console.WriteLine("Sorted set value: {0}", JsonConvert.SerializeObject(sortedSet));

            await db.HyperLogLogAddAsync("hll_key", 1);
            await db.HyperLogLogAddAsync("hll_key", 2);
            await db.HyperLogLogAddAsync("hll_key", 3);
            await db.HyperLogLogAddAsync("hll_key", 3);
            await db.HyperLogLogAddAsync("hll_key", 2);
            await db.HyperLogLogAddAsync("hll_key", 3);
            await db.HyperLogLogAddAsync("hll_key", 4);
            var hllCount = await db.HyperLogLogLengthAsync("hll_key");
            Console.WriteLine("HyperLogLog value: {0}", hllCount);
        }
    }
}
