using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public static class MyMath
    {
        public static int Max(params int[] numbers)
        {
            if (numbers.Length == 0)
            {
                throw new ArgumentException("Không có giá trị để so sánh", "numbers");
            }
            int max = numbers[0];
            foreach (var number in numbers)
            {
                if (number > max)
                {
                    max = number;
                }
            }
            return max;
        }
        public static int Min(params int[] numbers)
        {
            if (numbers.Length == 0)
            {
                throw new ArgumentException("Không có giá trị để so sánh", "numbers");
            }
            int min = numbers[0];
            foreach (var number in numbers)
            {
                if (number < min)
                {
                    min = number;
                }
            }
            return min;
        }
    }
}
