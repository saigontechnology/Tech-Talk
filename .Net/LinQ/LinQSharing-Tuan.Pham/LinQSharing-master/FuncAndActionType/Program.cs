using System.Runtime.CompilerServices;

namespace FuncAndActionType
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // delegate (pointer) to a method that takes zero or more input params, but returns NOTHING
            Action<int> myAction = new Action<int>(DoSomething);
            // delegate (pointer) to a method that takes zero or more input params, but returns A VALUE (OR REFERENCE)
            myAction(123);
            Func<int, double> myFunc = new Func<int, double>(CalculateSomething);
            Console.WriteLine(myFunc(3));
        }
        static void DoSomething(int i)
        {
            Console.WriteLine(i);
        }
        static double CalculateSomething(int i)
        {
            return (double)i/2;
        }
        
    }
}