using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicOperators
{
    [MemoryDiagnoser]
    public class MyBenchmark
    {
        public static string[] names = new[] { "David", "Diana", "Anna", "Chris", "Wayne", "John" };
        [Benchmark]
        public void QuerySyntax()
        {
            var filteredNames1 = from name in names
                                 where name.Contains("D") || name.Contains("C")
                                 where name.Length > 2
                                 select name;
        }
        [Benchmark]
        public void MethodSyntax()
        {
            var filteredNames2 = names
                .Where(name => name.Contains("D") || name.Contains("C"))
                .Where(name => name.Length > 2);
        }
    }
}
