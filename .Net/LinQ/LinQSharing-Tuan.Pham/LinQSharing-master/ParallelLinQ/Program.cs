using System.Diagnostics;
using System.Linq;

namespace ParallelLinQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 1000000);
            var stopwatch = new Stopwatch();

            // Measure time for LINQ query
            stopwatch.Start();
            var result1 = numbers.Where(n => n % 2 == 0).Sum();
            stopwatch.Stop();
            Console.WriteLine($"Time for LINQ query: {stopwatch.ElapsedMilliseconds} ms");

            // Measure time for PLINQ query
            stopwatch.Reset();
            stopwatch.Start();
            var result2 = numbers.AsParallel().Where(n => n % 2 == 0).Sum();
            stopwatch.Stop();
            Console.WriteLine($"Time for PLINQ query: {stopwatch.ElapsedMilliseconds} ms");


        }
    }
}