using System;
using System.Threading.Tasks;

class AsyncDeadlockExample
{
    static async Task<int> LongRunningTaskAsync()
    {
        Console.WriteLine("Long running task started");
        await Task.Delay(5000); // Simulate a long running task
        Console.WriteLine("Long running task completed");
        return 42;
    }

    static async Task<int> DeadlockTaskAsync()
    {
        Console.WriteLine("Deadlock task started");
        int result = await LongRunningTaskAsync();
        Console.WriteLine("Deadlock task completed");
        return result;
    }

    static async Task Main()
    {
        Console.WriteLine("Main thread started");
        Task<int> task1 = DeadlockTaskAsync();
        Task<int> task2 = DeadlockTaskAsync();
        int result1 = task1.Result;
        int result2 = await task2;
        Console.WriteLine($"Result 1 is {result1}");
        Console.WriteLine($"Result 2 is {result2}");
        Console.WriteLine("Main thread completed");
    }
}