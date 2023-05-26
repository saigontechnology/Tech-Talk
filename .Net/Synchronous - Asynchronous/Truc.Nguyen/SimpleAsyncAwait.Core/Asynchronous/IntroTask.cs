using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleAsyncAwait.Core.Asynchronous
{
    public static class IntroTask
    {
        public static void TaskInstantiation()
        {
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId + "\n");

            Task task1 = new Task(() =>
            {
                Console.WriteLine("Thread in task Start: " + Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Status task Start: " + task1.Status);
            task1.Start();
            Console.WriteLine("Status task Start: " + task1.Status);
            task1.Wait(2000);
            Console.WriteLine("Status task Start: " + task1.Status);

            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId + "\n");

            Task task2 = Task.Factory.StartNew(() => 
            {
                Console.WriteLine("Thread in task Factory.StartNew: " + Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Status task Factory.StartNew: " + task2.Status);
            task2.Wait(2000);
            Console.WriteLine("Status task Factory.StartNew: " + task2.Status);
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId + "\n");

            Task task3 = Task.Run(() =>
            {
                Console.WriteLine("Thread in task Run: " + Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Status task Run: " + task3.Status);
            task3.Wait(2000);
            Console.WriteLine("Status task Run: " + task3.Status);
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId + "\n");

            Task task4 = new Task(() =>
            {
                Console.WriteLine("Thread in task RunSynchronously: " + Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Status task RunSynchronously: " + task4.Status);
            task4.RunSynchronously();
            Console.WriteLine("Status task RunSynchronously: " + task4.Status);
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId + "\n");

        }

        public static async Task CommonWaysToCreateNewTask()
        {
            await Task.Run(() => { });

            int sum = await SumByFromResultAsync();
        }

        private static Task<int> SumByFromResultAsync() => Task.FromResult(1 + 2);


    }
}
