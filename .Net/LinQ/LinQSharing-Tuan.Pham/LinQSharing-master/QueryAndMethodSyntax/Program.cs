namespace QueryAndMethodSyntax
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // String collection
            IList<string> stringList = new List<string>()
            {
                "C# Tutorials",
                "VB.NET Tutorials",
                "Learn C++",
                "MVC TUtorials",
                "Java"
            };

            // Query Syntax
            var resultQuery = from s in stringList
                         where s.Contains("Tutorials")
                         select s;

            foreach (var s in resultQuery)
            {
                Console.WriteLine(s);
            }

            // Method Syntax
            var resultMethod = stringList.Where(s => s.Contains("Tutorials"));

            foreach (var s in resultMethod)
            {
                Console.WriteLine(s);
            }
            Console.Read();
        }
    }
}