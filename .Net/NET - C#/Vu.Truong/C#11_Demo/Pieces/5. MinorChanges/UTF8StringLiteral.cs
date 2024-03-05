using System.Text;

namespace C_11_Demo.MinorChanges;
internal class UTF8StringLiteral : IExample
{
    public static int Order => 9;

    public static string Name => nameof(UTF8StringLiteral);

    public static string Description => "UTF8 String Literal";

    public static void Execute()
    {
        ReadOnlySpan<byte> text = "Vũ Truong"u8;

        ReadOnlySpan<byte> u16 = Encoding.Unicode.GetBytes("A");
        ReadOnlySpan<byte> u8 = "A"u8;

        Console.WriteLine($"{u16.Length}");
        Console.WriteLine($"{u8.Length}");
        Console.WriteLine($"{Encoding.Unicode.GetString(text)}");
    }
}
