using System.Collections.Generic;
using System.Text.Json;

// After C# 12
using ListStrDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>;

namespace C_12_Demo.Pieces;

public class AliasAnyType : IExecFunction
{
    public static int Order => 2;

    public static string Name => nameof(AliasAnyType);

    public static string Description => "Alias Any Type";

    public static void Execute()
    {
        var map = new Dictionary<string, List<int>>();

        // After C# 12
        var map2 = new ListStrDict();

        Console.WriteLine($$"""
            var map = new Dictionary<string, List<int>>(); 
            Type: {{map.GetType()}}

            After C# 12
            var map2 = new ListStrDict();
            Type: {{map.GetType()}}
            """);
    }
}