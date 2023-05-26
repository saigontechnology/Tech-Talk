using BenchmarkDotNet.Running;
using RedisSharing.UseCases.ShoppingCart;

await Benchmarks.ResetAsync();

var summary = BenchmarkRunner.Run<Benchmarks>();
