global using System.Diagnostics.CodeAnalysis;
namespace C_10_Demo.Pieces;

public class AssignmentDeclarationInDeconstruction
{
    public static void Execute()
    {
        decimal x = 0;        

        var point = GetCenterPoint(0, 0, 10, 10);

        // Old
        (decimal first, decimal last) = point;

        (x, var y) = point;

        Console.WriteLine($"Center: {x}:{y}");

        int m = 0;

        TestPoint test = new();

        (var n, m) = test;

        Console.WriteLine($"Test point: {n} {m}");

        TestPointRec testRec = new(10, 5);

        (n, m) = testRec;

        Console.WriteLine($"Test point (record): {n} {m}");
    }

    public static (decimal X, decimal Y) GetCenterPoint(decimal X1, decimal Y1, decimal X2, decimal Y2)
    {
        return ((X2 + X1) / 2, (Y2 + Y1) / 2); 
    }
}

public class TestPoint
{
    public int N { get; set; }
    public int M { get; set; }

    public void Deconstruct(out int n, out int m)
    {
        n = N;
        m = M;
    }
}

public record TestPointRec(int N, int M);