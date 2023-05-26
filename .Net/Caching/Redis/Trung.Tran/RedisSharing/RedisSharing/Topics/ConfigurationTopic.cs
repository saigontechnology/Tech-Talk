namespace RedisSharing.Topics
{
    public class ConfigurationTopic
    {
        public static async Task StartAsync()
        {
            using var redis = Program.GetConnectionMultiplexer();
            var db = redis.GetDatabase();
            var server = redis.GetServer("localhost", 6379);

            await server.ConfigSetAsync("requirepass", "123123");

            await server.ConfigRewriteAsync();
        }
    }
}
