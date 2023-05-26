using BenchmarkDotNet.Running;
using RedisSharing.UseCases.ScoringTable;

await Benchmarks.ResetAsync();

var summary = BenchmarkRunner.Run<Benchmarks>();
