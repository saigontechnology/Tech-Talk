using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCSharp
{
    public static class ValueReferenceType
    {
        public static void ReferenceTypeWithString()
        {
            Console.WriteLine("--- Reference Type With String ---");
            string s1 = "String1";
            string s2 = "String1";
            Console.WriteLine("s1 = \"String1\";\ns2 = \"String1\";");
            Console.WriteLine("=> ReferenceEquals(s1, s2): " + ReferenceEquals(s1, s2));
            Console.WriteLine($"=> s1({s1}) interned: {(string.IsNullOrEmpty(string.IsInterned(s1)) ? "No" : "Yes")}");

            Console.WriteLine();
            string suffix = "A";
            string s3 = "String" + suffix;
            string s4 = "String" + suffix;
            Console.WriteLine("suffix = \"A\";\ns3 = \"String\" + suffix;\ns4 = \"String\" + suffix;");
            Console.WriteLine("=> ReferenceEquals(s3, s4): " + ReferenceEquals(s3, s4));
            Console.WriteLine($"=> s3({s3}) interned: {(string.IsNullOrEmpty(string.IsInterned(s3)) ? "No" : "Yes")}");
            Console.WriteLine();

            //string s5 = "StringA";
        }

        public static void ReferenceTypeWithObject()
        {
            Console.WriteLine("--- Reference Type With Object ---");
            Person o1 = new Person();
            Person o2 = new Person();
            Person o3 = o2;

            Console.WriteLine("o1 = new Person();\no2 = new Person();\no3 = o2;");
            Console.WriteLine("=> ReferenceEquals(o1, o2): " + ReferenceEquals(o1, o2));
            Console.WriteLine("=> ReferenceEquals(o2, o3): " + ReferenceEquals(o2, o3));


            o3 = new Person();
            Console.WriteLine("\no3 = new Person();");
            Console.WriteLine("=> ReferenceEquals(o1, o2): " + ReferenceEquals(o1, o2));
            Console.WriteLine("=> ReferenceEquals(o2, o3): " + ReferenceEquals(o2, o3));
            Console.WriteLine();
        }

        public static void ValueType()
        {
            Console.WriteLine("--- Value Type ---");
            int num1 = 3;
            Console.WriteLine("num1 = 3;      -> 0x239110: 3");
            int num2 = num1;
            Console.WriteLine("num2 = num1;   -> 0x239351: 3");
            Console.WriteLine("=> ReferenceEquals(num1, num2): " + ReferenceEquals(num1, num2));

            Console.WriteLine();
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
