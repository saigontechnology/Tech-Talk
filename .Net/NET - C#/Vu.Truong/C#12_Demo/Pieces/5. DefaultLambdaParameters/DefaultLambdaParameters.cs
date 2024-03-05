namespace C_12_Demo.Pieces;

public class DefaultLambdaParameters : IExecFunction
{
    public static int Order => 4;

    public static string Name => nameof(DefaultLambdaParameters);

    public static string Description => "Default Lambda Parameters";

    public static void Execute()
    {
        // Before C#12
        Func<int, int, int> add = (a, b) => a + b;

        // C# 12
        var add2 = (int a = 0, int b = 0) => a + b;

        var addWithDefault = (int addTo, int jump = 1) => addTo + jump;

        Console.WriteLine($$"""
            // Before C#12
            Func<int, int, int> add = (a, b) => a + b;
            Output:
            add(10, 5) = {{add(10, 5)}}

            // C# 12
            var addWithDefault = (int addTo, int jump = 1) => addTo + jump;
            Output:
            addWithDefault(10) = {{addWithDefault(10)}}
            addWithDefault(10, 2) = {{addWithDefault(10, 2)}}

            Func<int, int, int> add2 = (int a = 0, int b = 0) => a + b;
            Output:
            add2() = {{add2()}}
            add2(10) = {{add2(10)}}
            add2(10, 5) = {{add2(10, 5)}}
            """);
    }
}
