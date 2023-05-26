namespace BasicLinQ.Operators
{
    public static class Element
    {
        public static T FirstAndFirstDefault<T>(this IQueryable<T> source)
        {
            try
            {
                #region Log First()
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Get by First():");
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                return source.First();
            }
            catch (Exception ex)
            {
                #region Log First() ex
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("First() exception: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                #region Log First()
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Get by FirstOrDefault():");
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                return source.FirstOrDefault();
            }
        }

        public static T SingleAndSingleDefault<T>(this IQueryable<T> source)
        {
            try
            {
                #region Log Single()
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Get by Single():");
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                return source.Single();
            }
            catch (Exception ex)
            {
                #region Log Single() ex
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Single() exception: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                #region Log Single()
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Get by SingleOrDefault():");
                Console.ForegroundColor = ConsoleColor.Gray;
                #endregion

                return source.SingleOrDefault();
            }
        }
    }
}
