using Xunit;

namespace AdvancedTechniques.Tests.Unit;

[CollectionDefinition("My collection fixture", DisableParallelization = true)]
public class TestCollectionFixture : ICollectionFixture<MyClassFixture>
{

}
