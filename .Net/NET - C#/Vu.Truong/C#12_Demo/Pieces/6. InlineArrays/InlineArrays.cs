namespace C_12_Demo.Pieces;
public class InlineArrays : IExecFunction
{
    public static int Order => 5;

    public static string Name => nameof(InlineArrays);

    public static string Description => "Inline Arrays";

    public static void Execute()
    {
        var buffer = new Buffer<int>();
        var buffer2 = new Buffer<char>();

        for (int i = 0; i < 10; i++)
        {
            buffer[i] = i;
            buffer2[i] = (char)(i + 65);
        }

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"{buffer[i]} - {buffer2[i]}");
        }
    }
}

[System.Runtime.CompilerServices.InlineArray(10)]
public struct MyArray
{
    public int Value { get; set; }
}


[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer<T>
{
    private T _element0;
}