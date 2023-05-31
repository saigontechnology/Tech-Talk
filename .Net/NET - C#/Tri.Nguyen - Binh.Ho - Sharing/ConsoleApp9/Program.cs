using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringAndStringBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string FirstName = "Pro";
            String LastName = new String("Coder");

            Console.WriteLine("First Name - " + FirstName);
            Console.WriteLine("Last Name - " + LastName);

            Console.WriteLine(FirstName.GetType().FullName);
            Console.WriteLine(LastName.GetType().FullName);

            string FirstName2 = "Pro";
            StringBuilder LastName2 = new StringBuilder("Code");
            LastName2.Append(" Guide");
            Console.WriteLine("First Name - " + FirstName2);
            Console.WriteLine("Last Name - " + LastName2.ToString());
            Console.WriteLine(FirstName2.GetType().FullName);
            Console.WriteLine(LastName2.GetType().FullName);
        }
    }
}