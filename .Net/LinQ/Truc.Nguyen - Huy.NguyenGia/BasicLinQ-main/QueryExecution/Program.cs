using BenchmarkDotNet.Running;
using QueryExecution;

internal class Program
{
    private static void Main(string[] args)
    {

        var summary = BenchmarkRunner.Run<ExecutionType>();

        var executionType = new ExecutionType();
        executionType.ExecutionDeferred();
        executionType.ExecutionImmediate();
        executionType.ExecutionDeferredStreaming();
        executionType.ExecutionDeferredNonStreaming();
    }
}