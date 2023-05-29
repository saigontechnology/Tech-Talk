using LambdaExpressions;
using Models;

namespace LambdaExpressions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            IEnumerable<Employee> employees = new Employee[]
            {
                new Employee { Id = 1, Name = "abc"},
                new Employee { Id = 2, Name = "def"}
            };

            IEnumerable<Employee> filteredEmployeesWithFunc = employees.Where(NameStartingWithA);
            IEnumerable<Employee> filteredEmployeesWithDelegate = employees.Where(delegate(Employee employee)
            {
                return employee.Name.StartsWith("a");
            });
            IEnumerable<Employee> filteredEmployeesWithLambdaExpression = employees.Where(employee => employee.Name.StartsWith("a"));
            foreach (var employee in filteredEmployeesWithLambdaExpression)
            {
                Console.WriteLine(employee.Name);
            };
            Console.Read();
        }
        private static bool NameStartingWithA(Employee employee)
        {
            return employee.Name.StartsWith("a");
        }
    }
}


