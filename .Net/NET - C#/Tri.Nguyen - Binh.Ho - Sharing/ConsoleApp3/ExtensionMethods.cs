using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    static class ExtensionMethods
    {
        public static void ToConsole(this string message)
        {
            Console.WriteLine(message);
        }
        public static void ToConsole(this string message, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black, bool reset = true)
        {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.WriteLine(message);
            if (reset) Console.ResetColor();
        }
        public static double ToDouble(this string number)
        {
            return double.TryParse(number, out double d) ? d : double.NaN;
        }
        public static int ToInt(this string number)
        {
            return int.Parse(number);
        }
    }
}
