using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCSharp
{
    public class Equal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override bool Equals(object obj)
        {
            Equal p = (Equal)obj;
            if (p.Age == this.Age)
            {
                return true;
            }
            else return false;

        }

        public void Compare()
        {
            object str1 = "hello";
            char[] value2 = { 'h', 'e', 'l', 'l', 'o' };
            object str2 = new string(value2);
            Console.WriteLine("Using Equality operator: {0}", str1 == str2);
            Console.WriteLine("Using equals() method: {0}", str1.Equals(str2));

        }
    }
}
