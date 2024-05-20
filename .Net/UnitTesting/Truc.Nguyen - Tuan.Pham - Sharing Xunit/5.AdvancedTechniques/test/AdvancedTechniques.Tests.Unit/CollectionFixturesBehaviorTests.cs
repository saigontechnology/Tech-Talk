using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AdvancedTechniques.Tests.Unit;

[Collection("My collection fixture")]
public class CollectionFixturesBehaviorTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public CollectionFixturesBehaviorTests(ITestOutputHelper testOutputHelper,
        MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }
}

[Collection("My collection fixture")]
public class CollectionFixturesBehaviorTestsAgain
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public CollectionFixturesBehaviorTestsAgain(ITestOutputHelper testOutputHelper,
        MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_fixture.Id}");
        await Task.Delay(2000);
    }
}
