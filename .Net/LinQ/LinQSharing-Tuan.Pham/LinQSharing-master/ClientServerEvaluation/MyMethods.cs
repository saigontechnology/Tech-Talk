using ClientServerEvaluation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerEvaluation
{
    public static class MyMethods
    {
        public static void TopLevelProjection()
        {
            SchoolDbContext db = new SchoolDbContext();
            // Not throwing runtime exception
            var students = db.Students
            .OrderByDescending(s => s.FirstName)
            .Select(
                s => new { Name = (StandardizeName(s.FirstName)) })
            .ToList();
        }

        public static void UnsupportedClientEvaluation()
        {
            SchoolDbContext db = new SchoolDbContext();
            // Throwing runtime exception
            var students = db.Students
            .OrderByDescending(s => StandardizeName(s.FirstName))
            .Select(
                s => new { Name = s.FirstName })
            .ToList();
        }

        public static void ExplicitClientEvaluation()
        {
            SchoolDbContext db = new SchoolDbContext();
            // Convert to IEnumerable
            var query = db.Students.AsEnumerable();

            // Query is processed by LinQ in-memory operators
            var students = query
            .OrderByDescending(s => StandardizeName(s.FirstName))
            .Select(
                s => new { Name = s.FirstName })
            .ToList();

            foreach (var student in students)
            {
                Console.WriteLine(student.Name);
            }
        }

        public static void InmemoryProcess()
        {
            SchoolDbContext db = new SchoolDbContext();
            IQueryable<Student> query = db.Students.Where(s => s.Grade == 1).OrderBy(s => s.FirstName);

            // Server query
            IQueryable<Student> serverQuery = query.Skip(2).Take(1);
            // In-memory query
            IEnumerable<Student> inmemoryQuery = query.AsEnumerable().Skip(2).Take(1);

            // All the query is handled by IQueryable
            var serverResult = serverQuery.ToArray();
            // Handled by IQueryable: .Where(), .Orderby()
            // Converted to enumerable object
            // Handled by LinQ in-memory: .Skip(), .Take()
            var inmemoryResult = inmemoryQuery.ToArray();
        }

        private static string StandardizeName(string name)
        {
            name = name.ToLower();

            return name;
        }
    }
}
