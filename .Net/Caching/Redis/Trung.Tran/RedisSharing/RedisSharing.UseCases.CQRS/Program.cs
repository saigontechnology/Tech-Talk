using BenchmarkDotNet.Running;
using RedisSharing.UseCases.CQRS;

await Benchmarks.ResetAsync();

var summary = BenchmarkRunner.Run<Benchmarks>();
