using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleAsyncAwait.Core.Asynchronous
{
    public class PopularMethods
    {
        public static async Task ConfigureAwaitState()
        {

            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(threadId);
            await Task.Delay(1000).ConfigureAwait(true);

            int threadId2 = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(threadId2);
        }

        public static async Task TaskWhen(CancellationTokenSource cancellationTokenSource)
        {
            var timer = new Stopwatch();
            try
            {
                timer.Start();
                await Task1();
                await Task2();
                await Task3();

                //await Task.WhenAll(Task1(), Task2(), Task3());

                //var token = cancellationTokenSource.Token;
                //var task = await Task.WhenAny(Task1(token), Task2(token), Task3(token));
                //cancellationTokenSource.Cancel();

                //var task = Task.Run(async () =>
                //{
                //    while (!token.IsCancellationRequested)
                //    {
                //        Console.Write("*");
                //        await Task.Delay(500);
                //    }
                //    Console.WriteLine();
                //}, token);
                //await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cancellationTokenSource.Dispose();
                timer.Stop();
            }

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                await Cancel(cts.Token);
            }

            string timeTaken = "Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff");
            Console.WriteLine(timeTaken);
        }

        public static async Task TaskWait(CancellationTokenSource cancellationTokenSource)
        {
            var timer = new Stopwatch();
            try
            {
                timer.Start();

                Task.WaitAll(Task1(), Task2(), Task3());
                //Task.WaitAny(Task1(), Task2(), Task3());

                //var cancel = Cancel(cancellationTokenSource.Token).Result;
                //var cancel = Task.Run(() => Cancel(cancellationTokenSource.Token)).GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cancellationTokenSource.Dispose();
                timer.Stop();
            }

            string timeTaken = "Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff");
            Console.WriteLine(timeTaken);
        }

        private static async Task Task1(CancellationToken token = default)
        {
            await Task.Delay(1000, token);
            Console.WriteLine("Done Task 1 ");
        }
        private static async Task Task2(CancellationToken token = default)
        {
            await Task.Delay(2000, token);
            Console.WriteLine("Done Task 2 ");
        }
        private static async Task Task3(CancellationToken token = default)
        {
            await Task.Delay(3000, token);
            Console.WriteLine("Done Task 3 ");
        }

        private static async Task<int> Cancel(CancellationToken token)
        {
            Task.FromCanceled(token);

            return 3;
        }



        #region Create a task that has completed
        private static Task<int> TaskFromResult(int a, int b)
        {
            Task.Delay(2000);
            return Task.FromResult(a + b);
        }

        private static Task TaskFromCanceledOrCompleted(CancellationToken token)
        {
            Task.Delay(1500);

            if (token.IsCancellationRequested)
            {
                return Task.FromCanceled(token);
            }
            return Task.CompletedTask;
        }

        private static Task TaskFromException()
        {
            Task.Delay(3000);
            return Task.FromException(new TimeoutException());
        }

        #endregion
    }
}
