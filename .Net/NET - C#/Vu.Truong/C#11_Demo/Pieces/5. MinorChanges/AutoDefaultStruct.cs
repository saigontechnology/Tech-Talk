namespace C_11_Demo.MinorChanges;

internal struct MyStruct
{
    public int Number;
    public bool IsValid;
    public string Name;
    public DateTime Date;

    public MyStruct()
    {

    }
}

internal class AutoDefaultStruct : IExample
{
    public static int Order => 6;

    public static string Name => "Auto Default Struct";

    public static string Description => "Auto Default Struct";

    public static void Execute()
    {
        var myStruct = new MyStruct();

        Console.WriteLine($"{myStruct.Number} - {myStruct.IsValid} - {myStruct.Name} - {myStruct.Date}");
    }
}

