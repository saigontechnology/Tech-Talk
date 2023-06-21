// See https://aka.ms/new-console-template for more information
using BasicOperators;
using BenchmarkDotNet.Running;
using Models;
using System.Diagnostics;

var names = new[] { "David", "Diana", "Anna", "Chris", "Wayne", "John" };

// IEnumerable<string>
var allNames = from name in names
               select name;

allNames = names;

var stopwatch = new Stopwatch();
stopwatch.Start();
var filteredNames1 = from name in names
                    where name.Contains("D") || name.Contains("C")
                    where name.Length > 2
                    select name;
stopwatch.Stop();

var stopwatch1 = new Stopwatch();
stopwatch1.Start();
var filteredNames2 = names
    .Where(name => name.Contains("D") || name.Contains("C"))
    .Where(name => name.Length > 2);
stopwatch1.Stop();

var validNameLengths = new int[] { 3, 5 };

var complexNames = from name in names
                   select new
                   {
                       Name = name,
                       FirstLetter = name[0],
                       LastLetter = name[name.Length - 1],
                       NameLength = name.Length,
                       IsValidLength = (from nameLength in validNameLengths select nameLength)
                                       .Contains(name.Length)
                   };

Console.WriteLine("Filtered Names: ");
foreach (var filteredItem in filteredNames1)
{
    Console.WriteLine(filteredItem);
}

Console.WriteLine("==========================");
Console.WriteLine("Complex Names: ");
foreach (var complexItem in complexNames)
{
    Console.WriteLine(complexItem);
}

Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
Console.WriteLine(stopwatch1.Elapsed.TotalMilliseconds);

var summary = BenchmarkRunner.Run<MyBenchmark>();

Console.Read();