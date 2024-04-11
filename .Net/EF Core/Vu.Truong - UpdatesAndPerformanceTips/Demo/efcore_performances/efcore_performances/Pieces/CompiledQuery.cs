using efcore_performances.Pieces.Tests;
using System.Threading;

namespace efcore_performances.Pieces;
internal class CompiledQuery : IExample
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        BenchmarkDotNet.Running.BenchmarkRunner.Run<CompiledQueryBenchmark>();

        //await DbContextPoolingTest.Run(DbContextType.Normal, true, 128);
        //await DbContextPoolingTest.Run(DbContextType.Normal, false, 128);
    }
}
