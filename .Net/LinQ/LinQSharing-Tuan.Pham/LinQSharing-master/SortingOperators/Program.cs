using Models;

namespace SortingOperators
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

            //Query
            var orderByResult = from s in studentList
                                orderby s.Name
                                select s;
            //var orderByResult = studentList.OrderBy(x => x.Name);

            var orderByResultDescending = from s in studentList
                                orderby s.Name descending
                                select s;

            //var orderByResultDescending = studentList.OrderByDescending(x => x.Name);

            var orderByMultipleFields = from s in studentList
                                        orderby s.Name ascending, s.Age descending
                                        select s;

            //var orderByMultipleFields = studentList.OrderBy(x => x.Name).ThenByDescending(x => x.Age);

            Console.WriteLine("===============================");
            Console.WriteLine("Order by ascending examples");
            Console.WriteLine("===============================");
            foreach (var item in orderByResult)
            {
                Console.WriteLine($"Name: {item.Name}, Age: {item.Age}");
            }
            Console.WriteLine();
            Console.WriteLine("===============================");
            Console.WriteLine("Order by descending examples");
            Console.WriteLine("===============================");

            foreach (var item in orderByResultDescending)
            {
                Console.WriteLine($"Name: {item.Name}, Age: {item.Age}");
            }

            Console.WriteLine();
            Console.WriteLine("===============================");
            Console.WriteLine("Order by multiple fields examples");
            Console.WriteLine("===============================");

            foreach (var item in orderByMultipleFields)
            {
                Console.WriteLine($"Name: {item.Name}, Age: {item.Age}");
            }
            Console.Read();
        }
    }
}