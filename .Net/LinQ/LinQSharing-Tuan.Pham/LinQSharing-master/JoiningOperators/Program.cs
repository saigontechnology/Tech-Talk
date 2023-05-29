namespace JoiningOperators
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //Example 1
            IList<string> strList1 = new List<string>()
            {
                "1",
                "2",
                "3",
                "4"
            };
            IList<string> strList2 = new List<string>() 
            {
                "1",
                "2",
                "5",
                "6"
            };
            //Method syntax

            //var innerJoin = strList1.Join(strList2,
            //    str1 => str1,
            //    str2 => str2,
            //    (str1, str2) => str1); 
            
            //Query syntax
            
            var innerJoin1 = from str1 in strList1
                            join str2 in strList2 on str1 equals str2
                            select str1;
            foreach (var item1 in innerJoin1)
            {
                Console.WriteLine(item1);
            }
            Console.Read();


            //Example 2
            IList<Student> studentList = new List<Student>()
            {
                new Student() { Id = 1, Name = "Tom", Age = 23, ClassroomId = 1},
                new Student() { Id = 2, Name = "Edison", Age = 11, ClassroomId = 2},
                new Student() { Id = 3, Name = "John", Age = 20, ClassroomId = 1},
                new Student() { Id = 3, Name = "John", Age = 30, ClassroomId = 1},
                new Student() { Id = 4, Name = "Steven", Age = 28}
            };

            IList<Classroom> classroomList = new List<Classroom>()
            {
                new Classroom() { ClassroomId = 1, ClassroomName = "Math"},
                new Classroom() { ClassroomId = 2, ClassroomName = "Physics"},
                new Classroom() { ClassroomId = 3, ClassroomName = "Science"}
            };

            //var innerJoin2 = studentList.Join(classroomList,
            //    student => student.ClassroomId,
            //    classroom => classroom.ClassroomId,
            //    (student, classroom) => new
            //    {
            //        StudentName = student.Name,
            //        ClassroomName = classroom.ClassroomName
            //    });
            var innerJoin2 = from student in studentList
                             join classroom in classroomList on student.ClassroomId equals classroom.ClassroomId
                             select new
                             {
                                 StudentName = student.Name,
                                 ClassroomName = classroom.ClassroomName
                             };

            foreach (var item2 in innerJoin2)
            {
                Console.WriteLine($"Student Name: {item2.StudentName}");
                Console.WriteLine($"Classroom Name: {item2.ClassroomName}");
                Console.WriteLine("======================================");
            }

            //Left join example 2
            var leftJoin2 = from classroom in classroomList
                            join student in studentList on classroom.ClassroomId equals student.ClassroomId
                            into studentgroup
                            select new
                            {
                                Students = studentgroup,
                                ClassroomName = classroom.ClassroomName,
                            };

            var leftJoin3 = from student in studentList
                            join classroom in classroomList on student.ClassroomId equals classroom.ClassroomId
                            into gj
                            from subClass in gj.DefaultIfEmpty()
                            select new
                            {
                                StudentName = student.Name,
                                ClassroomName = subClass?.ClassroomName ?? string.Empty
                            };


            foreach (var item3 in leftJoin2)
            {
                Console.WriteLine(item3.ClassroomName);
                Console.WriteLine("******************");

                foreach (var stud in item3.Students)
                {
                    Console.WriteLine(stud.Name);
                }
            }

            foreach (var item3 in leftJoin3)
            {
                Console.WriteLine($"{item3.StudentName + ":",-15}{item3.ClassroomName}");
            }
        }
    }
}