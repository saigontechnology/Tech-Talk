using System;

namespace AdvancedTechniques.Tests.Unit;

public class MyClassFixture : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();

    public MyClassFixture()
    {

    }

    public void Dispose()
    {

    }
}
