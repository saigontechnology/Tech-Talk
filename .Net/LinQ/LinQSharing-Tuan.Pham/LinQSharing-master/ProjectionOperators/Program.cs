using Models;

namespace ProjectionOperators
{
    internal class Program
    {
        public class Classroom
        {
            public int Id { get; set; }
            public List<Student>? Students { get; set; }
        }
        static void Main(string[] args)
        {
            // Select example
            IList<Student> studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "Tom", Age = 23},
                new Student() { Id = 2, Name = "Edison", Age = 11},
                new Student() { Id = 3, Name = "John", Age = 20},
                new Student() { Id = 4, Name = "Ram", Age = 30}
            };
            var selectedStudentList = studentList.Select(x => new
            {
                Name = $"Mr.{x.Name}",
                x.Age,
            });
            foreach (var item in selectedStudentList)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Age);
            }

            // SelectMany example
            IList<Classroom> classroomList = new List<Classroom>()
            {
                new Classroom()
                {
                    Id = 1, Students = new List<Student>()
                    {
                        new Student() { Id = 1, Name = "Tom", Age = 23},
                        new Student() { Id = 2, Name = "Edison", Age = 11},
                        new Student() { Id = 3, Name = "John", Age = 20},
                        new Student() { Id = 4, Name = "Ram", Age = 30}
                    }
                }
            };
            var selectedStudentClassroomList = classroomList.SelectMany(x => x.Students ?? 
            new List<Student>() { new Student() { Id = 1, Name = "null", Age = 23 } });

            foreach (var classItem in selectedStudentClassroomList)
            {
                Console.WriteLine(classItem.Name);
            }
        }
    }
}