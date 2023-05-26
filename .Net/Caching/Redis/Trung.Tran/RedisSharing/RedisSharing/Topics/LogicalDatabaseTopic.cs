using Newtonsoft.Json;

namespace RedisSharing.Topics
{
    public static class LogicalDatabaseTopic
    {
        public static async Task StartAsync()
        {
            using var redis = Program.GetConnectionMultiplexer();
            var server = redis.GetServer("localhost", 6379);
            var db1 = redis.GetDatabase(1);
            await db1.StringSetAsync("my_new_key", "my_new_value");

            var db1Keys = server.Keys(1).Select(k => k.ToString());
            Console.WriteLine("Database 1 keys: {0}", JsonConvert.SerializeObject(db1Keys));

            var defaultDb = redis.GetDatabase();
            var defaultKeys = server.Keys().Select(k => k.ToString());
            Console.WriteLine("Default database (db 0) keys: {0}", JsonConvert.SerializeObject(defaultKeys));
        }
    }
}
