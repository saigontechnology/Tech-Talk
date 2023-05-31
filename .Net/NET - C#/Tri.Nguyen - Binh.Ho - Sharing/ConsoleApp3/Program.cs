using ConsoleApp3;
using static System.Console;
using static ConsoleApp3.MyMath;
class Program
{
    static void Main(string[] args)
    {
        int[] numbers = new[] { 1, 2, 3, 4, 5, 6 };
        // có thể gọi Max và Min theo cách ngắn gọn vì đã có using static MyMath; ở trên
        int max = Max(numbers);
        int min = Min(numbers);
        // có thể gọi WriteLine ngắn gọn vì đã có using static Console; ở trên
        WriteLine($"Max value: {max}");
        WriteLine($"Min value: {min}");

        "Hello world!".ToConsole();
        "Hello again!".ToConsole(ConsoleColor.Magenta);
        int i = "2000".ToInt();
        double d = "2000.0001".ToDouble();

        ReadKey();
    }
}