using System.ComponentModel;

namespace C_11_Demo.MinorChanges;

internal class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {

    }
}

internal class ExtendedNameOf
{
    public static int Order => 9;

    public static string Name => nameof(ExtendedNameOf);

    public static string Description => "Extended NameOf";

    [DisplayName(nameof(name))]
    public void Update(string name)
    {
        var expr = ([Name(nameof(cost))] int cost) => Console.WriteLine(cost);
    }

    [Name(nameof(T))]
    public void Info<T>(T name)
    {
        
    }

    public static void Execute()
    {
        throw new NotImplementedException();
    }
}
