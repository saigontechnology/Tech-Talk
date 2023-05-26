using RedisSharing.Topics;
using StackExchange.Redis;

//await GettingStartedTopic.StartAsync();

//await DataTypeTopic.StartAsync();

//await LogicalDatabaseTopic.StartAsync();

await ConfigurationTopic.StartAsync();

public partial class Program
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