using BenchmarkDotNet.Attributes;
using QueryExecution.ExtensionMethods;

namespace QueryExecution
{
    [MemoryDiagnoser]
    public class ExecutionType
    {
        public static IEnumerable<Supplier> suppliers => Enumerable.Range(1, 200000)
                    .Select(i => new Supplier { Id = i, Name = $"supplier {i}" });

        public void ExecutionDeferred()
        {
            var supplierFilterred = suppliers.WhereExecutionStreaming(x => x.Id <= 2);

            Console.WriteLine("Deferred Streaming");
            foreach (var supplier in supplierFilterred)
            {
                Console.WriteLine("Supplier Id: " + supplier.Id);
            }

            Console.WriteLine("\nDeferred Non-Streaming 1");
            foreach (var supplier in supplierFilterred.ToList())
            {
                Console.WriteLine("Supplier Id: " + supplier.Id);
            }

            Console.WriteLine("\nDeferred Non-Streaming 2");
            foreach (var supplier in supplierFilterred.ToList())
            {
                Console.WriteLine("Supplier Id: " + supplier.Id);
            }
            Console.WriteLine();
        }

        public void ExecutionImmediate()
        {
            Console.WriteLine("Immediate");
            var supplierFilterred = suppliers.WhereExecutionStreaming(x => x.Id <= 2).ToList();

            Console.WriteLine("Immediate 1");
            foreach (var supplier in supplierFilterred)            {
                Console.WriteLine("Supplier Id: " + supplier.Id);
            }

            Console.WriteLine("Immediate 2");
            foreach (var supplier in supplierFilterred.ToList())
            {
                Console.WriteLine("Supplier Id: " + supplier.Id);
            }
            Console.WriteLine();
        }
        
        public void ExecutionDeferredStreaming()
        {
            var supplierFilterred = suppliers.WhereExecutionStreamingWithoutLog(x => x.Id <= 2);

            Console.WriteLine("Deferred Streaming");
            foreach (var supplier in supplierFilterred)
            {
                //Console.WriteLine("Supplier Id: " + supplier.Id);
            }
            Console.WriteLine();
        }
        
        public void ExecutionDeferredNonStreaming()
        {
            var supplierFilterred = suppliers.WhereExecutionStreamingWithoutLog(x => x.Id <= 2);
          
            Console.WriteLine("\nDeferred Non-Streaming 1");
            foreach (var supplier in supplierFilterred.ToList())
            {
                //Console.WriteLine("Supplier Id: " + supplier.Id);
            }
            Console.WriteLine();
        }
    }
}
