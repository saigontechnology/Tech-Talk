using RedisSharing.UseCases.ServerBackedSession.Helpers;

/// <summary>
/// [Important] Suitable for load balancing (alternative of Sticky session) 
/// Replacement for relational session data store, remove overhead for database roundtrip per request
/// Support auto expiration to reduce redundant data
/// Others: can be other temporary, short-lived data
/// NOTE: Can be integrated with ASP.NET Core Session (using IDistributedCache)
/// </summary>

var redisMultiplexer = RedisHelper.GetConnectionMultiplexer();

var server = redisMultiplexer.GetServer("localhost", 6379);

await server.FlushDatabaseAsync();

for (var i = 0; i < 100; i++)
{
    await ThreadHelper.RunMultipleAsync(10, async () =>
    {
        var db = redisMultiplexer.GetDatabase();
        var trans = db.CreateTransaction();
        var sessionId = $"session:{Guid.NewGuid()}";
        var tasks = new List<Task>();
        tasks.Add(trans.HashSetAsync(sessionId, "Item1", Guid.NewGuid().ToString()));
        tasks.Add(trans.HashSetAsync(sessionId, "Item2", Guid.NewGuid().ToString()));
        tasks.Add(trans.HashSetAsync(sessionId, "Item3", Guid.NewGuid().ToString()));
        tasks.Add(trans.HashSetAsync(sessionId, "Item4", Guid.NewGuid().ToString()));
        tasks.Add(trans.ExecuteAsync("EXPIRE", sessionId, 10));
        await trans.ExecuteAsync();

        //await db.StringSetAsync(sessionId, JsonConvert.SerializeObject(new
        //{
        //    Item1 = Guid.NewGuid(),
        //    Item2 = Guid.NewGuid(),
        //    Item3 = Guid.NewGuid(),
        //    Item4 = Guid.NewGuid(),
        //}), expiry: TimeSpan.FromSeconds(10));
    });
}
