using ConsoleTables;

namespace BasicLinQ
{
    public static class Helper
    {
        public static void LogListData<T>(IEnumerable<T> list)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Log table:");
            Console.ForegroundColor = ConsoleColor.Gray;
            ConsoleTable.From(list).Write();
        }
    }
}
