using Models;

namespace GroupingOperators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<Student> studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "Tom", Age = 23},
                new Student() { Id = 2, Name = "Edison", Age = 11},
                new Student() { Id = 3, Name = "John", Age = 20},
                new Student() { Id = 3, Name = "John", Age = 30}
            };

            var groupedResult = from s in studentList
                                group s by s.Age;

            foreach (var ageGroup in groupedResult)
            {
                Console.WriteLine($"Age: {ageGroup.Key}");
                
                foreach (var s in ageGroup)
                {
                    Console.Write($"Student name: { s.Name}");
                }
            }
        }
    }
}