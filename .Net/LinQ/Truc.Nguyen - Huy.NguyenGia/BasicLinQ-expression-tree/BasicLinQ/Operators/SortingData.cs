using BasicLinQ.Entities;

namespace BasicLinQ.Operators
{
    public static class SortingData
    {
        public static IEnumerable<Product> AscendingSort(this IEnumerable<Product> source)
        {
            #region Log OrderBy()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Sort by OrderBy():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.OrderBy(x => x.Id);
        }
        public static IEnumerable<Product> DescendingSort(this IEnumerable<Product> source)
        {
            #region Log OrderByDescending()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Sort by OrderByDescending():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return from src in source
                   orderby src.Id descending
                   select src;
        }

        public static IEnumerable<Product> AscendingThenByDescendingSort(this IEnumerable<Product> source)
        {
            #region Log OrderBy() + ThenByDescending()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Sort by OrderBy() + ThenByDescending():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.OrderBy(x => x.CategoryId).ThenByDescending(x => x.Name);
        }
        public static IEnumerable<Product> DescendingThenByAscendingSort(this IEnumerable<Product> source)
        {
            #region Log OrderByDescending() + ThenBy()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Sort by OrderByDescending() + ThenBy():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return from src in source
                   orderby src.CategoryId descending, src.Name
                   select src;
        }
    }
}
