using BenchmarkDotNet.Running;
using RedisSharing.UseCases.Caching;

// [Important] should cache data with read frequency higher than write's

await Benchmarks.ResetAsync();

var summary = BenchmarkRunner.Run<Benchmarks>();
