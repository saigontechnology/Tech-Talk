using System;

namespace C_11_Demo.MinorChanges;
internal class SpanPatternMatching : IExample
{
    public static int Order => 7;

    public static string Name => "Pattern match Span for constant string";

    public static string Description => "Pattern match Span for constant string";
    public static void Execute()
    {
        Console.WriteLine("""Test with "This is span" as ReadOnlySpan<char>""");
        ReadOnlySpan<char> pattern = "This is span";

        Console.WriteLine();
        Console.WriteLine($"""Check if span is "This is span": { (pattern is "This is span" ? "matched" : "not matched") } """);

        Console.WriteLine();
        Console.WriteLine($"""Check if span is [.., ' ', 's', 'p', 'a', 'n']: {(pattern is [.., ' ', 's', 'p', 'a', 'n'] ? "It contains 'span'" : "not matched")} """);
    }
}
