// See https://aka.ms/new-console-template for more information
using RedisSharing.UseCases.DistributedLock.Helpers;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

// [Important] should FlushDB first for easier observation

var redisMultiplexer = RedisHelper.GetConnectionMultiplexer();

await LockVer(redisMultiplexer);

await NoLockVer(redisMultiplexer);

static async Task LockVer(ConnectionMultiplexer connectionMultiplexer)
{
    var multiplexers = new List<RedLockMultiplexer>
    {
        connectionMultiplexer
    };

    using var redlockFactory = RedLockFactory.Create(multiplexers);

    var db = connectionMultiplexer.GetDatabase();
    var resource = "count-lock";
    var expiry = TimeSpan.FromSeconds(1);
    var timeout = TimeSpan.FromSeconds(10);
    var retry = TimeSpan.FromMilliseconds(100);

    for (var i = 0; i < _loop; i++)
    {
        await Task.Delay(new Random().Next(250));

        await using (var redLock = await redlockFactory.CreateLockAsync(resource, expiry, timeout, retry)) // there are also non async Create() methods
        {
            if (redLock.IsAcquired)
            {
                await IncrementAsync(db, "locked");
            }
        }
    }

}

static async Task NoLockVer(ConnectionMultiplexer connectionMultiplexer)
{
    var multiplexers = new List<RedLockMultiplexer>
    {
        connectionMultiplexer
    };

    using var redlockFactory = RedLockFactory.Create(multiplexers);

    var db = connectionMultiplexer.GetDatabase();

    for (var i = 0; i < _loop; i++)
    {
        await Task.Delay(new Random().Next(250));

        await IncrementAsync(db, "no-locked");
    }

}

static async Task IncrementAsync(IDatabase db, string key)
{
    var lockedCountStr = await db.StringGetAsync(key);
    int lockedCount = 0;

    if (!string.IsNullOrEmpty(lockedCountStr))
    {
        // [Important] not atomic (demo only)
        // Can be replaced by SQL Server or other store that required distributed lock
        lockedCount = int.Parse(lockedCountStr);
    }

    await Task.Delay(new Random().Next(250));

    await db.StringSetAsync(key, ++lockedCount);

    Console.WriteLine($"{key}: {lockedCount}");
}

partial class Program
{
    static readonly int _loop = 50;
}