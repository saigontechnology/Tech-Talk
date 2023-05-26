namespace BasicLinQ.Operators
{
    public static class Quantifier
    {
        public static bool CheckAllExisted<T>(this IQueryable<T> source, Func<T, bool> predicate)
        {
            #region Log All()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Filter by All():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.All(predicate);
        }
        public static bool CheckExisted<T>(this IQueryable<T> source, Func<T, bool> predicate)
        {
            #region Log Any()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Filter by Any():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.Any(predicate);
        }
        public static bool CheckExisted<T>(this IQueryable<T> source)
        {
            #region Log Any()
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Filter by Any():");
            Console.ForegroundColor = ConsoleColor.Gray;
            #endregion

            return source.Any();
        }
        //public static bool CheckContain<T>(IQueryable<T> source, string term)
        //{
        //    return source.Contains(term);
        //}
    }
}
