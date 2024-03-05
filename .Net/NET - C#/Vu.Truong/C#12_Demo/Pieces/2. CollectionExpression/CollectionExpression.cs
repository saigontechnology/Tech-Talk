namespace C_12_Demo.Pieces;
public class CollectionExpression : IExecFunction
{
    public static int Order => 1;

    public static string Name => nameof(CollectionExpression);

    public static string Description => "Collection Expressions";

    public static void Execute()
    {
        // Before C# 12
        int[] x1 = new int[] { 1, 2, 3, 4 };
        List<int> x2 = new List<int>() { 1, 2, 3, 4 };
        Span<int> x3 = stackalloc int[] { 1, 2, 3, 4 };

        Console.WriteLine($$"""
            Before C# 12:
            int[] x1 = new int[] { 1, 2, 3, 4 };
            List<int> x2 = new List<int>() { 1, 2, 3, 4 };
            Span<int> x3 = stackalloc int[] { 1, 2, 3, 4 };

            Output:
            Array 1:
            {{JsonSerializer.Serialize(x1)}}

            Array 2:
            {{JsonSerializer.Serialize(x2)}}

            Array 3:
            {{JsonSerializer.Serialize(x3.ToArray())}}
            """);

        // With C# 12
        int[] x11 = [1, 2, 3, 4];
        List<int> x12 = [1, 2, 3, 4];
        Span<int> x13 = [1, 2, 3, 4];

        Console.WriteLine($$"""

            After C# 12: 
            int[] x11 = [1, 2, 3, 4];
            List<int> x12 = [1, 2, 3, 4];
            Span<int> x13 = [1, 2, 3, 4];

            Output:

            Array 1:
            Array 1:
            {{JsonSerializer.Serialize(x11)}}
            
            Array 2:
            {{JsonSerializer.Serialize(x12)}}
            
            Array 3:
            {{JsonSerializer.Serialize(x13.ToArray())}}
            """);

        // Using the spread operator
        int[] numbers1 = [1, 2, 3];

        int[] numbers2 = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        Console.WriteLine($$"""


            int[] numbers2 = {{JsonSerializer.Serialize(numbers2)}}

            Output for numbers2[.. ^2]:
            {{JsonSerializer.Serialize(numbers2[..^2])}}

            Output for numbers2[2 .. ^2]:
            {{JsonSerializer.Serialize(numbers2[2 ..^2])}}

            Output for numbers2[2 .. ]:
            {{JsonSerializer.Serialize(numbers2[2 ..])}}
            """);

        List<int> listNumbers = [.. numbers1, .. numbers2];

        int[] allNumbers = [.. numbers1, .. numbers2, 7, 8, 9];


        var sumFunc = (params int[] numbers) => numbers.Sum();

        int[] sumNumbers = [.. numbers1, 5, 12, .. numbers2[..^2]];

        Console.WriteLine($$"""

            Spread operator: 
            int[] numbers1 = [1, 2, 3];
            int[] numbers2 = [4, 5, 6];
            int[] moreNumbers = [.. numbers1, .. numbers2, 7, 8, 9];

            allNumbers: 
            {{JsonSerializer.Serialize(allNumbers)}}

            Sum - sumFunc([.. numbers1, 5, 12, .. numbers2[.. 2]]) :

            {{JsonSerializer.Serialize(sumNumbers)}}

            {{sumFunc([.. numbers1, 5, 12, .. numbers2[.. ^2]])}}

            """);
    }
}

file record Employee(string firstName, string lastName, int grade);