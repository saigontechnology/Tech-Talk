using Models;
using System.Diagnostics.CodeAnalysis;

namespace QuantifierOperators
{
    internal class Program
    {
        class StudentComparer : IEqualityComparer<Student>
        {
            public bool Equals(Student? x, Student? y)
            {
                if (x?.Id == y?.Id && x?.Name?.ToLower() == y?.Name?.ToLower() && x?.Age == y?.Age)
                    return true;
                return false;
            }

            public int GetHashCode([DisallowNull] Student obj)
            {
                return obj.GetHashCode();
            }
        }
        static void Main(string[] args)
        {
            IList<Student> studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "Tom", Age = 23},
                new Student() { Id = 2, Name = "Edison", Age = 13},
                new Student() { Id = 3, Name = "John", Age = 20},
                new Student() { Id = 4, Name = "Ram", Age = 30}
            };

            Student std = new Student() { Id = 1, Name = "Tom", Age = 23 };

            bool areAllStudentsTeenagers = studentList.All(x => x.Age > 12 && x.Age < 20);
            bool isAnyStudentTeenagers = studentList.Any(x => x.Age > 12 && x.Age < 20);
            bool containReference = studentList.Contains(std, new StudentComparer());

            Console.WriteLine(areAllStudentsTeenagers);
            Console.WriteLine(isAnyStudentTeenagers);
            Console.WriteLine(containReference);
        }
    }
}