using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    internal class ExceptionHandling
    {
        public static void Example1() 
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n)
            .Select(n => 10 / n);

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        public static void Example2()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n);
            
            try
            {
                numbers = numbers.Select(n => 10 / n);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        public static void Example3()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n)
            .Select(n => 10 / n);

            try
            {
                foreach (var n in numbers)
                {
                    Console.WriteLine(n);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void Example4()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n)
            .Select(n =>
            {
                try
                {
                    return 10 / n;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return 123;
                }
            });

            numbers = numbers.Where(n => n != 123);

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        public static void Example5()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n)
            .Select(n =>
            {
                try
                {
                    return 10 / n;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return 123;
                }
            });

            numbers = numbers.Where(n => n != 123);

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        public static void Example6()
        {
            var numbers = Enumerable.Range(1, 10)
            .Select(n => 5 - n)
            .TrySelect(n => 10 / n, ex => Console.WriteLine($"Error: {ex.Message}"));

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
