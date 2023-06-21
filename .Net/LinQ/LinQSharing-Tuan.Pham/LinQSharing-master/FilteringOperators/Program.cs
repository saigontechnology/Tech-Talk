using Models;
using System.Collections;

namespace FilteringOperators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<Student> studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "John", Age = 13},
                new Student() { Id = 2, Name = "Molly", Age = 21},
                new Student() { Id = 3, Name = "Bill", Age = 18},
                new Student() { Id = 4, Name = "Ram", Age = 20},
                new Student() { Id = 5, Name = "Rom", Age = 15},
            };

            // Where: Returns values from the collection based on a predicate function
            // Query Syntax
            Console.WriteLine("===============================");
            Console.WriteLine("Where Examples");
            Console.WriteLine("===============================");
            var filteredResultQuery = from s in studentList
                                 where s.Age > 12
                                 && s.Age < 20
                                 select s;
            // Method Syntax
            var filteredResultMethod = studentList.Where(x => x.Age > 12 && x.Age < 20);

            foreach (var queryItem in filteredResultQuery)
            {
                Console.WriteLine(queryItem.Name);
            }
            foreach (var methodItem in filteredResultMethod)
            {
                Console.WriteLine(methodItem.Name);
            }

            // OfType: Returns values from the collection based on a specified type
            Console.WriteLine("===============================");
            Console.WriteLine("OfType Examples");
            Console.WriteLine("===============================");
            IList mixedList = new ArrayList();
            mixedList.Add(0);
            mixedList.Add("Zero");
            mixedList.Add("Three");
            mixedList.Add(3);
            mixedList.Add(new Student() { Id = 6, Name = "Neptune" });

            var stringResult = from s in mixedList.OfType<string>()
                                 select s;
            // var stringResult = mixedList.OfType<string>();
            foreach (var s in stringResult)
            {
                Console.WriteLine(s);
            }

            var intResult = from i in mixedList.OfType<int>()
                            select i;
            foreach (var i in intResult)
            {
                Console.WriteLine(i);
            }
        }
    }
}