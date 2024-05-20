using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AdvancedTechniques.Tests.Unit;

public class DefaultBehaviorTests
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;

    public DefaultBehaviorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }
}


public class DefaultBehaviorTests2
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;

    public DefaultBehaviorTests2(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }
}