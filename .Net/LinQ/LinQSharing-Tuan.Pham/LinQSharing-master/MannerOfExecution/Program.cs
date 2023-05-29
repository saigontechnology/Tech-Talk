namespace MannerOfExecution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> dataStream = DataStream();

            Console.WriteLine("Immediate execution");
            IEnumerable<string> filteredStream = dataStream.Where(s =>
            {
                Console.WriteLine("Filtering ...");
                return true;
            });
            int count = filteredStream.Count();
            IEnumerable<string> cachedDataStream = filteredStream.ToList();
            foreach (var data in cachedDataStream) Console.WriteLine(data);
            count = cachedDataStream.Count();
            Console.WriteLine($"Count: {count}");
            Console.WriteLine();

            Console.WriteLine("Deferred: Streaming");
            var deferredStream = filteredStream.Select(s => s);
            foreach (var data in deferredStream) Console.WriteLine(data);
            foreach (var data in deferredStream) Console.WriteLine(data);
            Console.WriteLine();

            Console.WriteLine("Deferred: Non-Streaming");
            var orderedStream = filteredStream.OrderBy(s =>
            {
                Console.WriteLine("Ordering ...");
                return s;
            });
            Console.WriteLine("Run 1");
            foreach (var data in orderedStream) Console.WriteLine(data);
            Console.WriteLine("Run 2");
            foreach (var data in orderedStream) Console.WriteLine(data);
            Console.WriteLine();
        }

        static IEnumerable<string> DataStream()
        {
            yield return "A123";

            Console.WriteLine("Processing ..."); Thread.Sleep(1000);

            yield return "B123";

            Console.WriteLine("Processing ..."); Thread.Sleep(1000);

            yield return "C123";

            Console.WriteLine("Stream end!");
        }

    }
 }
