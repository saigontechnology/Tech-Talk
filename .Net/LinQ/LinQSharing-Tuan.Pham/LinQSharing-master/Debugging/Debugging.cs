using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugging
{
    internal class Debugging
    {
        public static void Example1()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => n * n)
            .Select(n => n / 2)
            .Select(n => n - 5);
            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        public static void Example2()
        {
            var numbers = Enumerable.Range(1, 10).ToList();
            var squared = numbers.Select(n => n * n).ToList();
            var halved = numbers.Select(n => n / 2).ToList();
            var minusFive = numbers.Select(n => n - 5).ToList();
            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
