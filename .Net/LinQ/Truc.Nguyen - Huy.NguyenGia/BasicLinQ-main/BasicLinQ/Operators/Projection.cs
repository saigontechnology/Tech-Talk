using System.Linq.Expressions;

namespace BasicLinQ.Operators
{
    public static class Projection
    {
        public static IQueryable<TResult> SelectSimple<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            #region Log Select()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Projecting by Select():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.Select(selector);
        }
        public static IQueryable<TResult> SelectManySimple<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            #region Log SelectMany()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Projecting by SelectMany():");
            Console.ForegroundColor = ConsoleColor.Gray;
            # endregion

            return source.SelectMany(selector);
        }
    }
}
