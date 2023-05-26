using BenchmarkDotNet.Running;
using RedisSharing.UseCases.HitCount;

await Benchmarks.ResetAsync();

var summary = BenchmarkRunner.Run<Benchmarks>();
