using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCSharp
{
    public abstract class AbstractJob
    {
        public AbstractJob()
        {

        }

        public abstract String GetJobName();

        public abstract void DoJob();

        public virtual void DoSomething()
        {
            Console.WriteLine("Do....");
        }
    }

    public class JavaCoding : AbstractJob
    {
        public override void DoJob()
        {
            Console.WriteLine("Coding Java...");
        }

        public override string GetJobName()
        {
            return "Java Coding";
        }
    }

    public class CSharpCoding : AbstractJob
    {
        public override void DoJob()
        {
            Console.WriteLine("Coding C#...");
        }

        public override string GetJobName()
        {
            return "C# Coding";
        }

        public override void DoSomething()
        {
            Console.WriteLine("Do Coding C#");
        }
    }

    public abstract class ManualJob : AbstractJob
    {
        public ManualJob()
        {

        }
        public override string GetJobName()
        {
            return "Manual Job";
        }
    }

    public class BuildHouse : ManualJob
    {
        public override void DoJob()
        {
            Console.WriteLine("Build a House");
        }
    }

    interface IShape
    {
        int GetArea();
        int GetPerimeter();
    }

    class Rectangle: IShape
    {
        int a;
        int b;

        public int GetArea()
        {
            return a ^ b ;
        }

        public int GetPerimeter()
        {
            return (a + b) * 2;
        }
    }


}
