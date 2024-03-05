using System.Diagnostics;

namespace C_12_Demo.Pieces;
public class RefReadonlyParameters : IExecFunction
{
    public static int Order => 3;

    public static string Name => nameof(RefReadonlyParameters);

    public static string Description => "Ref Readonly Parameters";

    public static void Execute()
    {
        Point p1 = new Point(1, 1);
        Point p2 = new Point(2, 2);

        unsafe
        {
            Point* ptr = &p2;

            Console.WriteLine($"Address of p2 ({p2.X}-{p2.Y}) is {(int)ptr}");
        }

        double distance = p1.DistanceTo(p2); // Copies p2 by value

        unsafe
        {
            Point* ptr = &p2;

            Console.WriteLine($"Address of p2 ({p2.X}-{p2.Y}) is {(int)ptr}");
        }

        double distance2 = p1.DistanceToWithRefReadonly(ref p2); // Passes p2 by reference, but prevents modification

        unsafe
        {
            Point* ptr = &p2;

            Console.WriteLine($"Address of p2 ({p2.X}-{p2.Y}) is {(int)ptr}");
        }

        double distance3 = p1.DistanceTo(ref p2); // Passes p2 by reference, but allows modification

        unsafe
        {
            Point* ptr = &p2;

            Console.WriteLine($"Address of p2 ({p2.X}-{p2.Y}) is {(int)ptr}");
        }
    }
}
file struct Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Before C# 12
    public double DistanceTo(Point target)
    {
        unsafe
        {
            Point* ptr = &target;
            Console.WriteLine($"Address of target is {(int)ptr}");

            target.X = 10;
        }

        return Math.Sqrt((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y));
    }

    public double DistanceTo(ref Point target)
    {
        unsafe
        {
            fixed (Point* ptr = &target)
            {
                Console.WriteLine($"(Ref) Address of target is {(int)ptr}");

                target.X = 10;
            }
        }

        return Math.Sqrt((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y));
    }

    // With C# 12
    public double DistanceToWithRefReadonly(ref readonly Point target)
    {
        unsafe
        {
            fixed (Point* ptr = &target)
            {
                Console.WriteLine($"(Ref Readonly) Address of target is {(int)ptr}");
            }

            // we cannot do this:
            // target.X = 10;
        }

        return Math.Sqrt((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y));
    }
}
