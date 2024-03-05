namespace C_10_Demo.Pieces;
public class LambdaExpressionImprovement
{
    public static void Execute()
    {
        // func
        var checkOdd = (int number) => number % 2 != 0;

        // action
        var printOdd = (int number) =>
        {
            Console.WriteLine($"{number} is {(checkOdd(number) ? "odd" : "even")}");
        };

        // return type for func
        var defaultException = Exception (bool b) => b ? new ArgumentNullException() : new ApplicationException();

        // add attribute to lambda
        var concat = ([DisallowNull] string a, [DisallowNull] string b) => a + b;
        var increase = [return: NotNullIfNotNull(nameof(s))] (int? s) => s.HasValue ? s++ : null;

        Console.Write("Enter a number: ");
        var numberStr = Console.ReadLine();
        int.TryParse(numberStr, out var number);

        printOdd(number);
    }
}
