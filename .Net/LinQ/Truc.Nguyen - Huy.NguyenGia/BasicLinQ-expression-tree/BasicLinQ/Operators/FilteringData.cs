using System.Collections;
using System.Linq.Expressions;

namespace BasicLinQ.Operators
{
    public static class FilteringData
    {
        public static IQueryable<T> WhereCustomize<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            #region Log Where()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Filter by Where():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion
            return source.Where(predicate);
        }

        public static IEnumerable<T> CheckOfType<T>(this IEnumerable source)
        {
            //var product = 
            return source.OfType<T>();
        }
    }
}
