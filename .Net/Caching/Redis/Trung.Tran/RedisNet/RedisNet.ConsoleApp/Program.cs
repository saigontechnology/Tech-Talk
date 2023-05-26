// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

//await ConnectAsync();

//await CrudAsync();

//await PubSubAsync();

//await TransactionAsync();

//await ScriptAsync();

//await BackupAsync();

//await SetAuthAsync();

//await BatchVsPipelineAsync();

//await ConfigPersistenceAsync();

//await KeyspaceEventAsync();

//await ReadReplicationAsync();

//await LRUCacheAsync();

await TlsAsync();

static async Task TlsAsync()
{
    using var redis = GetTlsConnection();
    var db = redis.GetDatabase();

    var allKeys = await db.ExecuteAsync("KEYS", "*");
    Console.WriteLine(JsonConvert.SerializeObject((string[])allKeys));

    await db.StringSetAsync("author", "TNT", expiry: TimeSpan.FromMinutes(1));
    var author = await db.StringGetAsync("author");
    Console.WriteLine(author);
}

static async Task LRUCacheAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var server = redis.GetServer("localhost", 6379);

    await server.FlushAllDatabasesAsync();
    await Task.Delay(1000);
    await server.ConfigSetAsync("maxmemory", "4mb");
    await server.ConfigSetAsync("maxmemory-policy", "allkeys-lru");

    var dataBuilder = new StringBuilder("");
    Enumerable.Range(0, 1000).ToList().ForEach(x =>
        dataBuilder.Append(Guid.NewGuid().ToString()));
    var data = dataBuilder.ToString();

    for (var i = 0; i < 1000; i++)
    {
        await db.StringSetAsync($"key-{i}", data);
    }

    var count = await server.DatabaseSizeAsync();
    Console.WriteLine(count);
}

static async Task ConfigPersistenceAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var server = redis.GetServer("localhost", 6379);

    await server.ConfigSetAsync("save", "60 10");

    await server.ConfigSetAsync("appendonly", "yes");

    await server.ConfigRewriteAsync();
}

static async Task KeyspaceEventAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var server = redis.GetServer("localhost", 6379);

    await server.ConfigSetAsync("notify-keyspace-events", "KEA");

    var subscriber = redis.GetSubscriber();

    await subscriber.SubscribeAsync("__key*__:*", (channel, value) =>
    {
        Console.WriteLine($"From channel {channel}: {value}");
    });

    Console.WriteLine("Listening");
    Console.ReadLine();
}

static async Task BatchVsPipelineAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();

    var batch = db.CreateBatch();
    var _ = batch.StringSetAsync("b1", "okay");
    _ = batch.StringSetAsync("b2", "okay");
    _ = batch.StringSetAsync("b3", "okay");
    batch.Execute();

    var pipelines = new[]
    {
        db.StringSetAsync("b1", "okay"),
        db.StringSetAsync("b2", "okay"),
        db.StringSetAsync("b3", "okay"),
    };
    await Task.WhenAll(pipelines);
}

static async Task SetAuthAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var defaultEndpoint = redis.GetEndPoints().FirstOrDefault();
    var server = redis.GetServer(defaultEndpoint);

    await server.ConfigSetAsync("requirepass", "123456");
    await server.ConfigRewriteAsync();
}

static async Task BackupAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var defaultEndpoint = redis.GetEndPoints().FirstOrDefault();
    var server = redis.GetServer(defaultEndpoint);

    await server.SaveAsync(SaveType.BackgroundSave);

    var result = await server.ConfigGetAsync("dir");
    Console.WriteLine(JsonConvert.SerializeObject(result.ToDictionary()));
}

static async Task ScriptAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();

    var script = LuaScript.Prepare("return {@Key1, @Value1}");
    var result = await db.ScriptEvaluateAsync(script, new
    {
        Key1 = "Key1",
        Value1 = "Value1",
    });

    var json = JsonConvert.SerializeObject(result.ToDictionary());
    Console.WriteLine(json);
}

static async Task TransactionAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();

    await db.KeyDeleteAsync(new RedisKey[] { "transaction-1" });

    var trans = db.CreateTransaction();

    // [Important] increase or decrease to see the effect of transaction
    for (int i = 0; i < 1000; i++)
    {
        object _ = trans.ListRightPushAsync("transaction-1", "1");
    }

    var executedTask = trans.ExecuteAsync();

    var task = Task.Run(async () =>
    {
        using var redis = GetConnectionMultiplexer();
        var db = redis.GetDatabase();

        await db.ListRightPushAsync("transaction-1", "2");
        Console.WriteLine("Pushed 2");
    });

    var commited = await executedTask;

    if (commited)
    {
        var length = await db.ListLengthAsync("transaction-1");
        Console.WriteLine("Transaction 1 length: {0}", length);

        var leftValue = await db.ListLeftPopAsync("transaction-1");
        Console.WriteLine("Left value: {0}", leftValue);

        var rightValue = await db.ListRightPopAsync("transaction-1");
        Console.WriteLine("Right value: {0}", rightValue);
    }
    else
    {
        Console.WriteLine("Rollback");
    }
}

static async Task PubSubAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var server = redis.GetServer("localhost", 6379);

    var subscriber = redis.GetSubscriber();

    await subscriber.SubscribeAsync("a-channel", (channel, value) =>
    {
        Console.WriteLine($"From channel {channel}: {value}");
    });

    foreach (var number in Enumerable.Range(0, 10))
    {
        await subscriber.PublishAsync("a-channel", $"Some value {number}");
        await Task.Delay(500);
    }
}

static async Task ConnectAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();
    var server = redis.GetServer("localhost", 6379);

    var result = await db.ExecuteAsync("PING");
    var json = JsonConvert.SerializeObject(result);
    Console.WriteLine(json);

    result = await db.ExecuteAsync("CONFIG", "GET", "loglevel");
    json = JsonConvert.SerializeObject(result.ToDictionary());
    Console.WriteLine(json);

    result = await server.ExecuteAsync("CONFIG", "GET", "*");
    json = JsonConvert.SerializeObject(result.ToDictionary());
    Console.WriteLine(json);

    redis.Dispose();

    using var aRedis = GetConnectionMultiplexer();
    db = aRedis.GetDatabase();
    server = aRedis.GetServer("localhost", 6379);
    //result = await server.ExecuteAsync("CONFIG", "SET", "loglevel", "notice");
    await server.ConfigSetAsync("loglevel", "notice");
}

static async Task ReadReplicationAsync()
{
    using var redis = GetReplicationConnectionMultiplexer();
    var db = redis.GetDatabase();

    var allKeys = await db.ExecuteAsync("KEYS", "*");
    Console.WriteLine(JsonConvert.SerializeObject((string[])allKeys));

    var author = await db.StringGetAsync("author");
    Console.WriteLine(author);

    var person = await db.HashGetAllAsync("person");
    Console.WriteLine(JsonConvert.SerializeObject(person.ToDictionary()));

    var allNumbers = await db.ListRangeAsync("numbers");
    Console.WriteLine(JsonConvert.SerializeObject(allNumbers));

    var numberSet = await db.SetMembersAsync("set-numbers");
    Console.WriteLine(JsonConvert.SerializeObject(numberSet));

    var sortedSet = await db.SortedSetRangeByRankAsync("sorted-set-numbers");
    Console.WriteLine(JsonConvert.SerializeObject(sortedSet));

    var hllCount = await db.HyperLogLogLengthAsync("hll");
    Console.WriteLine(hllCount);
}

static async Task CrudAsync()
{
    using var redis = GetConnectionMultiplexer();
    var db = redis.GetDatabase();

    var allKeys = await db.ExecuteAsync("KEYS", "*");
    Console.WriteLine(JsonConvert.SerializeObject((string[])allKeys));

    await db.StringSetAsync("author", "TNT", expiry: TimeSpan.FromMinutes(1));
    var author = await db.StringGetAsync("author");
    Console.WriteLine(author);

    await db.HashSetAsync("person", new[]
    {
        new HashEntry("name","TNT"),
        new HashEntry("age","22"),
    });
    var person = await db.HashGetAllAsync("person");
    Console.WriteLine(JsonConvert.SerializeObject(person.ToDictionary()));

    await db.KeyDeleteAsync("numbers");
    await db.ListRightPushAsync("numbers", new RedisValue[] { 1, 2, 3, 4, 5, 6, "ABC" });
    var allNumbers = await db.ListRangeAsync("numbers");
    Console.WriteLine(JsonConvert.SerializeObject(allNumbers));

    await db.SetAddAsync("set-numbers", new RedisValue[] { 1, 1, 2, 2, 3, 3, "ABC" });
    var numberSet = await db.SetMembersAsync("set-numbers");
    Console.WriteLine(JsonConvert.SerializeObject(numberSet));

    await db.KeyDeleteAsync("sorted-set-numbers");
    await db.SortedSetAddAsync("sorted-set-numbers", new[]
    {
        new SortedSetEntry("TNT", 1),
        new SortedSetEntry("ABC", 2),
        new SortedSetEntry("XYZ", 3),
        new SortedSetEntry("ABC", 4),
    });
    var sortedSet = await db.SortedSetRangeByRankAsync("sorted-set-numbers");
    Console.WriteLine(JsonConvert.SerializeObject(sortedSet));

    await db.HyperLogLogAddAsync("hll", 1);
    await db.HyperLogLogAddAsync("hll", 2);
    await db.HyperLogLogAddAsync("hll", 3);
    await db.HyperLogLogAddAsync("hll", 3);
    await db.HyperLogLogAddAsync("hll", 2);
    await db.HyperLogLogAddAsync("hll", 3);
    await db.HyperLogLogAddAsync("hll", 4);
    var hllCount = await db.HyperLogLogLengthAsync("hll");
    Console.WriteLine(hllCount);
}

static ConnectionMultiplexer GetConnectionMultiplexer()
{
    var cfg = new ConfigurationOptions
    {
        User = "admin",
        Password = "123456",
        AllowAdmin = true,
    };
    cfg.EndPoints.Add("localhost");
    var connStr = cfg.ToString();
    return ConnectionMultiplexer.Connect(connStr);
}

static ConnectionMultiplexer GetReplicationConnectionMultiplexer()
{
    var cfg = new ConfigurationOptions
    {
        User = "admin",
        Password = "123456",
        AllowAdmin = true,
    };
    cfg.EndPoints.Add("localhost", 6379);
    cfg.EndPoints.Add("localhost", 6380);
    var connStr = cfg.ToString();
    return ConnectionMultiplexer.Connect(connStr);
}

static ConnectionMultiplexer GetTlsConnection()
{
    var cfg = new ConfigurationOptions
    {
        User = "admin",
        Password = "123456",
        AllowAdmin = true,
        Ssl = true,
        SslHost = "Server-only",
    };
    var certFile = @"..\..\..\..\RedisVolume\cert\client.pfx";
    var certificate = new X509Certificate2(certFile, "");
    cfg.CertificateSelection += delegate
    {
        return certificate;
    };
    cfg.EndPoints.Add("localhost", 6381);
    return ConnectionMultiplexer.Connect(cfg);
}
