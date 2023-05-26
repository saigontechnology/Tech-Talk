using StackExchange.Redis;

namespace RedisSharing.UseCases.ServerBackedSession.Helpers
{
    public static class RedisHelper
    {
        public static ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var cfg = new ConfigurationOptions
            {
                User = "admin",
                Password = "123456",
                AllowAdmin = true,
            };
            cfg.EndPoints.Add("localhost");
            var connStr = cfg.ToString();
            return ConnectionMultiplexer.Connect(cfg);
        }
    }
}
