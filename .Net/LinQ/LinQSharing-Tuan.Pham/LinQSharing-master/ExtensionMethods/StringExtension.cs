using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StringExtension
    {
        public static double ToDouble (this string data)
        {
            bool result = Double.TryParse(data, out double number);
            return number;
        } 
    }
}
