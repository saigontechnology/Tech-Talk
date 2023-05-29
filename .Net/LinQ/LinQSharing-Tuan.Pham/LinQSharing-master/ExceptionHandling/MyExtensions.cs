using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    static class MyExtensions
    {
        public static IEnumerable<TResult> TrySelect<TSource, TResult>(this IEnumerable<TSource> source,
                                                                        Func<TSource, TResult> selector,
                                                                        Action<Exception> exceptionAction)
        {
            foreach (var s in source)
            {
                TResult result = default(TResult);
                bool success = false;
                try
                {
                    result = selector(s);
                    success = true;
                }
                catch (Exception ex)
                {
                    exceptionAction(ex);
                }
                if (success)
                {
                    // can't yield return inside a try block
                    yield return result;
                }
            }
        }
    }
}
