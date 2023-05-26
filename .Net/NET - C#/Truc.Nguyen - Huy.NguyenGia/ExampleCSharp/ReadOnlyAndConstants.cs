using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCSharp
{
    public class ReadOnlyAndConstants
    {
        const string myConstant = "Constants are cool!";
        public double Circumference(double radius)
        {

            return 2 * System.Math.PI * radius;
        }

        public class SampleClass
        {
            public int x;
            // Initialize a readonly field
            public readonly int y = 25;
            public readonly int z;

            public SampleClass()
            {
                // Initialize a readonly instance field
                z = 24;
                y = 35; // you can change y value here
            }

            public SampleClass(int p1, int p2, int p3)
            {
                x = p1;
                y = p2;
                z = p3;
            }
        }
    }
}
