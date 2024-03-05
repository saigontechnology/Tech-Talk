// See https://aka.ms/new-console-template for more information
using C_10_Demo.Pieces;

while (true)
{

    Console.Clear();

    Console.WriteLine("C# 10 Demo");
    Console.Write("Enter a function by number: ");
    Console.Out.Flush();
    var input = (Console.ReadLine() ?? "").ToLower();

    switch (input)
    {
        case "0":
            ConstantsInterpolatedStrings.Execute();
            break;
        case "1":
            ExtendedPropertyPatterns.Execute();
            break;
        case "2":
            LambdaExpressionImprovement.Execute();
            break;
        case "3":
            AssignmentDeclarationInDeconstruction.Execute();
            break;
        default:
            return;
    }

    Console.Write("Press any key to continue...");
    Console.ReadKey();
}