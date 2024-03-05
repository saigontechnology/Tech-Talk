namespace C_11_Demo.Pieces;
public class ListPatterns : IExample
{
    public static int Order => 1;

    public static string Name => "List Pattern";

    public static string Description => "List Pattern";

    public static void Execute()
    {
        var numbers = new int[] { 1, 2, 5 };
        Console.WriteLine("Test numbers: { 1, 2, 5 }");

        Console.WriteLine($"Numbers is [1, 2, 5]: {numbers is [1, 2, 5]}");
        Console.WriteLine($"Numbers is [1, 2, 2]: {numbers is [1, 2, 2]}");
        Console.WriteLine($"Numbers is [1, 2, 2, 3, 5]: {numbers is [1, 2, 2, 3, 5]}");
        Console.WriteLine($"Numbers is [1, 2, ..]: {numbers is [1, 2, ..]}");
        Console.WriteLine($"Numbers is [.., 2, 5]: {numbers is [.., 2, 5]}");
        Console.WriteLine($"Numbers is [.., 4]: {numbers is [.., 4]}");

        Console.WriteLine($"Numbers is [0 or 1 or 2, < 3, >= 4]: {numbers is [0 or 1 or 2, < 3, >= 4]}");

        if (numbers is [var first, .. var rest])
        {
            Console.WriteLine($"First number: {first}");
        }

        Console.WriteLine();
        Console.WriteLine("Pattern with switch expression: ");

        var emptyName = new string[0];
        var theName = new string[] { "Vu Truong" };
        var theNameBroken = new string[] { "Vu", "TruongP" };
        var theNameBroken2 = new string[] { "Vu", "Truong", "2024" };
        var theNameFourMember = new string[] { "Vu", "Truong", "2024", "Techtalk" };
        var largeName = new string[] { "Vu", "Truong", "2024", "Techtalk", "Mar", "C#" };

        var nameMaker = (string[] pattern) =>
        {
            var name = pattern switch
            {
                [] => "Empty",
                [string fullName] => $"(1) {fullName}",
                [string firstName, string lastName] => $"(2) {firstName} {lastName}",
                [..] when pattern.Length > 3 && pattern.Length <= 5 => "Array more than 3 members",
                [..] when pattern.Length > 5 => "Large array",
                _ => "Default case..."
            };

            return name;
        };

        Console.WriteLine($"Name []: {nameMaker(emptyName)}");
        Console.WriteLine($"Name [Vu Truong]: {nameMaker(theName)}");
        Console.WriteLine($"Name [Vu, TruongP]: {nameMaker(theNameBroken)}");
        Console.WriteLine($"Name [Vu, Truong, 2024]: {nameMaker(theNameBroken2)}");
        Console.WriteLine($"Name [Vu, Truong, 2024, Techtalk]: {nameMaker(theNameFourMember)}");
        Console.WriteLine($"Name [Vu, Truong, 2024, Techtalk, Mar, C#]: {nameMaker(largeName)}");
    }
}