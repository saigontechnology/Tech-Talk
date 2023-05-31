using System;
namespace Overriding
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public double Salary { get; set; }

        public virtual double CalculateBonus(double Salary)
        {
            return 50000;
        }
    }

    public class Developer : Employee
    {
        //50000 or 20% Bonus to Developers which is greater
        public override double CalculateBonus(double Salary)
        {
            double baseSalry = base.CalculateBonus(Salary);
            double calculatedSalary = Salary * .20;
            if (baseSalry >= calculatedSalary)
            {
                return baseSalry;
            }

            else
            {
                return calculatedSalary;
            }
        }
    }

    public class Manager : Employee
    {
        //50000 or 25% Bonus to Developers which is greater
        public override double CalculateBonus(double Salary)
        {
            double baseSalry = base.CalculateBonus(Salary);
            double calculatedSalary = Salary * .25;
            if (baseSalry >= calculatedSalary)
            {
                return baseSalry;
            }
            else
            {
                return calculatedSalary;
            }
        }
    }

    public class Admin : Employee
    {
        //return fixed bonus 50000
        //no need to overide the method
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Employee emp1 = new Developer
    //        {
    //            Id = 1001,
    //            Name = "Ramesh",
    //            Salary = 500000,
    //            Designation = "Developer"
    //        };
    //        double bonus = emp1.CalculateBonus(emp1.Salary);
    //        Console.WriteLine($"Name: {emp1.Name}, Designation: {emp1.Designation}, Salary: {emp1.Salary}, Bonus:{bonus}");
    //        Console.WriteLine();

    //        Employee emp2 = new Manager
    //        {
    //            Id = 1002,
    //            Name = "Sachin",
    //            Salary = 800000,
    //            Designation = "Manager"
    //        };
    //        bonus = emp2.CalculateBonus(emp2.Salary);
    //        Console.WriteLine($"Name: {emp2.Name}, Designation: {emp2.Designation}, Salary: {emp2.Salary}, Bonus:{bonus}");
    //        Console.WriteLine();

    //        Employee emp3 = new Admin
    //        {
    //            Id = 1003,
    //            Name = "Rajib",
    //            Salary = 300000,
    //            Designation = "Admin"
    //        };
    //        bonus = emp3.CalculateBonus(emp3.Salary);
    //        Console.WriteLine($"Name: {emp3.Name}, Designation: {emp3.Designation}, Salary: {emp3.Salary}, Bonus:{bonus}");
    //        Console.WriteLine();

    //        Employee emp4 = new Developer
    //        {
    //            Id = 1004,
    //            Name = "Priyanka",
    //            Salary = 200000,
    //            Designation = "Developer"
    //        };
    //        bonus = emp1.CalculateBonus(emp4.Salary);
    //        Console.WriteLine($"Name: {emp4.Name}, Designation: {emp4.Designation}, Salary: {emp4.Salary}, Bonus:{bonus}");

    //        Console.Read();
    //    }
    //}
}