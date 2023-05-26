using BasicLinQ.Entities;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

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

        public static IEnumerable<T> WhereExecution<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            #region Log Where()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Filter by WhereExecution():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion
            int index = -1;
            foreach (T element in source)
            {
                index ++;
                Console.WriteLine("Element " + index);

                if (predicate(element))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> CheckOfType<T>(this IEnumerable source)
        {
            //var product = 
            return source.OfType<T>();
        }

        public static IQueryable<T> Get<T>(this IQueryable<T> source)
        {
            if (source == null)
                throw new Exception("Source can be null");
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
            {
                PropertyInfo prop = typeof(T).GetProperty("IsDeleted");
                var parameter = Expression.Parameter(typeof(T));
                var property = Expression.Property(parameter, prop);
                var condition = Expression.Equal(Expression.Constant(false), property);
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

                return source.Where(lambda);
            }

            return source;
        }
    }
}
