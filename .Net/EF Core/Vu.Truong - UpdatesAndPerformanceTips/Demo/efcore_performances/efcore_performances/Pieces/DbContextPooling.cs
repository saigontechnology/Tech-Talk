using efcore_performances.Pieces.Tests;

namespace efcore_performances.Pieces;
internal class DbContextPooling : IExample
{
    const int MaxThreads = 64;
    static string[] AgreedValues = ["y", "yes", "ok", "dứt"];

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        int threads = MaxThreads;
        Console.Write("Enter number of threads: ");
        int.TryParse(Console.ReadLine(), out threads);

        bool useCompiledQuery = false;
        Console.Write("Use compiled query? (y/n): ");
        var input = Console.ReadLine();
        if (AgreedValues.Contains(input.ToLower()))
        {
            useCompiledQuery = true;
        }

        await DbContextPoolingTest.Run(DbContextType.Normal, useCompiledQuery, threads);

        await DbContextPoolingTest.Run(DbContextType.Pooling, useCompiledQuery, threads);

        //This test is for db context factory and pooled factory, u can try urself
        //await DbContextPoolingTest.Run(DbContextType.Factory, useCompiledQuery, threads);

        //await DbContextPoolingTest.Run(DbContextType.PooledFactory, useCompiledQuery, threads);
    }
}
