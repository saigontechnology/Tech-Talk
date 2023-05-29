// See https://aka.ms/new-console-template for more information
using Example1;
using ExtensionMethods;
using Models;

// Test string extension method
//Console.WriteLine("Hello, World!");
//string text = "43.35";
//double doubleTest = text.ToDouble();
//Console.WriteLine(doubleTest);

// Test enumerable extension method
IEnumerable<Employee> employeeList = new Employee[]
{
    new Employee(){ Id = 1, Name = "ABC"},
    new Employee(){ Id = 2, Name = "DEF"}
};
Console.WriteLine(employeeList.CountTest());

Console.Read();