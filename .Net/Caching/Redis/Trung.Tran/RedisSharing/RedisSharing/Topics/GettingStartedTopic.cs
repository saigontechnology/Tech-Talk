using Newtonsoft.Json;

namespace RedisSharing.Topics
{
    public static class GettingStartedTopic
    {
        public static async Task StartAsync()
        {
            using var redis = Program.GetConnectionMultiplexer();
            var db = redis.GetDatabase();
            var server = redis.GetServer("localhost", 6379);

            var result = await db.ExecuteAsync("PING");
            var json = JsonConvert.SerializeObject(result);
            Console.WriteLine(json);

            await db.StringSetAsync("my_key", "my_value");

            result = await server.ExecuteAsync("CONFIG", "GET", "*");
            json = JsonConvert.SerializeObject(result.ToDictionary());
            Console.WriteLine(json);

            await server.ConfigSetAsync("loglevel", "notice");
        }
    }
}
