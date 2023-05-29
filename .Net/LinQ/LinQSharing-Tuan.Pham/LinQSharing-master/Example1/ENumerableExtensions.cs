using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    public static class ENumerableExtensions
    {
        public static int CountTest<T>(this IEnumerable<T> enumerable)
        {
            int count = 0;
            foreach (var item in enumerable)
            {
                count++;
            }
            return count;
        }
        public static string StringConcat<T>(this IEnumerable<T> strings, string separator)
        {
            return string.Join(separator, strings);
        }
    }
}
