using System.Numerics;

namespace C_11_Demo.Pieces;
public class GenericMathSupport : IExample
{
    public static int Order => 4;

    public static string Name => "Generic Math";

    public static string Description => "Generic Math";

    public static void Execute()
    {
        int[] sets = new int[] { 1, 2, 3 };
        var sum = Sum(sets);
        var addAll = AddAll(sets);

        Charge[] pSets = sets.Select(x => new Charge(x, 1)).ToArray();
        var sum2 = Sum(pSets);

        Console.WriteLine($"Sum use IAdditionOperators: {sum}");
        Console.WriteLine($"Sum use INumber: {addAll}");
        Console.WriteLine($"Sum Charge Record: {sum2}");

        var charge1 = new Charge(0, 50);
        var newCharge = Concat(charge1, 10);

        Console.WriteLine($"Sum Charge Record and Int: {newCharge}");
    }

    public static T Sum<T>(T[] source)
        where T : IAdditionOperators<T, T, T>
    {
        if (source is null || !source.Any())
            throw new ArgumentNullException();

        var span = source.AsSpan();
        var sum = span[0];

        for (var i = 1; i < span.Length; i++)
        {
            sum += span[i];
        }

        return sum;
    }

    public static T AddAll<T>(T[] source) where T : INumber<T>
    {
        if (source is null || !source.Any())
            throw new ArgumentNullException();

        T result = T.Zero;

        for (var i = 0; i < source.Length; i++)
        {
            result += source[i];
        }

        return result;
    }

    public static T Concat<T, U>(T first, U last) where T : IAdditionOperators<T, U, T>
    {
        return first + last;
    }
}

internal record Charge(int cost, int fee) : IAdditionOperators<Charge, Charge, Charge>, IAdditionOperators<Charge, int, Charge>
{
    public static Charge operator +(Charge left, Charge right)
    {
        return new Charge(left.cost + right.cost, left.fee + right.fee);
    }

    public static Charge operator +(Charge left, int right)
    {
        return new Charge(left.cost + right, left.fee + right);
    }
}