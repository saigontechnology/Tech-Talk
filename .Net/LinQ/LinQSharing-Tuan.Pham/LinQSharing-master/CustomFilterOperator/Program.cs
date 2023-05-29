using Models;

namespace CustomFilterOperator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Student collection
            IEnumerable<Student> list = new List<Student>()
            {
                new Student() { Id = 1 , Name = "Minh Quang"},
                new Student() { Id = 2, Name = "Minh Pham"},
                new Student() { Id = 3, Name = null}
            };

            var resultWithCustomFilter = list.Filter(x => x.Name != null ? x.Name.Contains("Minh") : false);
            //var resultWithCustomFilter = list.Where(x => x.Name != null ? x.Name.Contains("Minh") : false);
            //var resultWithCustomFilter = list.FilterYield(x => x.Name != null ? x.Name.Contains("Minh") : false);
            foreach (var result in resultWithCustomFilter)
            {
                Console.WriteLine(result.Name);
            }
            Console.Read();
        }
    }
}