namespace C_11_Demo.MinorChanges;
internal class StringInterpolationNewLine : IExample
{
    public static int Order => 8;

    public static string Name => "String Interpolation in new line";

    public static string Description => "String Interpolation in new line";

    public static void Execute()
    {
        var number = 3;
        var text = $"You are {number switch
                {
                    < 1 => "junior",
                    < 4 => "intermediate",
                    _ => "strong"
                }
            } developer";

        Console.WriteLine(text);
    }
}
