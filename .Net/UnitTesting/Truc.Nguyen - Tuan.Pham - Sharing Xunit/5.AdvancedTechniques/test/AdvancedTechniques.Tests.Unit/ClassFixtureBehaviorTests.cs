using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AdvancedTechniques.Tests.Unit;

public class ClassFixtureBehaviorTests : IClassFixture<MyClassFixture>
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public ClassFixtureBehaviorTests(ITestOutputHelper testOutputHelper,
        MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }
}
