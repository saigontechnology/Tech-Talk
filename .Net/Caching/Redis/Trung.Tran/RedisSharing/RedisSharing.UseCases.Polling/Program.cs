
using RedisSharing.UseCases.Polling.Helpers;
using StackExchange.Redis;

var server = RedisMultiplexer.GetServer("localhost", 6379);

await server.FlushDatabaseAsync();

var subTask = Task.Run(SubscribeAsync);
var pollTask = Task.Run(PollAsync);

await StartAsync();

await Task.WhenAll(subTask, pollTask);

static async Task StartAsync()
{
    var sub = RedisMultiplexer.GetSubscriber();
    var db = RedisMultiplexer.GetDatabase();
    var random = new Random().Next(101);

    for (var i = 1; i <= random; i++)
    {
        var delay = new Random().Next(500);
        await Task.Delay(delay);
        var progress = (double)i / random * 100;
        await sub.PublishAsync("progress", progress);
        await db.StringSetAsync("progress", progress);
    }
}

static async Task SubscribeAsync()
{
    var sub = RedisMultiplexer.GetSubscriber();
    await sub.SubscribeAsync("progress", (channel, progress) =>
    {
        var progressVal = (int)double.Parse(progress);
        SubProgress = progressVal;
        PrintResult();
    });
}

static async Task PollAsync()
{
    var db = RedisMultiplexer.GetDatabase();

    do
    {
        var pollStr = await db.StringGetAsync("progress");

        if (!string.IsNullOrWhiteSpace(pollStr))
        {
            var pollVal = double.Parse(pollStr);
            PollProgress = (int)pollVal;
            PrintResult();
        }

        await Task.Delay(1000);
    } while (PollProgress <= 99);
}

static void PrintResult()
{
    lock (_lock)
    {
        Console.SetCursorPosition(0, 0);
        var subStr = $"Subscribe: {SubProgress}%";
        Console.WriteLine(subStr);
        var pollStr = $"Polling: {PollProgress}%";
        Console.WriteLine(pollStr);
    }
}

partial class Program
{
    static readonly object _lock = new object();
    static int SubProgress = 0;
    static int PollProgress = 0;

    static readonly ConnectionMultiplexer RedisMultiplexer = RedisHelper.GetConnectionMultiplexer();
}
