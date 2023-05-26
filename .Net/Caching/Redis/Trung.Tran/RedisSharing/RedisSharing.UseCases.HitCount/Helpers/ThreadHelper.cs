namespace RedisSharing.UseCases.HitCount.Helpers
{
    public static class ThreadHelper
    {
        static readonly object _increaseLock = new object();

        public static int ThreadSafeIncrease(ref int count)
        {
            return Interlocked.Increment(ref count);
        }

        public static async Task RunMultipleAsync(int count, Func<Task> action)
        {
            await Task.WhenAll(Enumerable.Range(0, count).Select(x => Task.Run(action)));
        }
    }
}
