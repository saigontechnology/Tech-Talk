using System.Threading.Tasks;
using Xunit;

namespace AdvancedTechniques.Tests.Unit;

public class LongRunningTests
{
    [Fact(Timeout = 2000)]
    public async Task SlowTest()
    {
        await Task.Delay(10000);
    }
}
