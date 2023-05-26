using System;
using static ExampleCSharp.ReadOnlyAndConstants;

namespace ExampleCSharp
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Value-Reference Type
            //ValueReferenceType.ReferenceTypeWithString();
            //ValueReferenceType.ReferenceTypeWithObject();
            //ValueReferenceType.ValueType();
            #endregion

            #region Readolny
            //ReadOnly
            //SampleClass s1 = new SampleClass(11, 21, 32);   // OK
            //Console.WriteLine("p1: x={0}, y={1}, z={2}", s1.x, s1.y, s1.z);
            //SampleClass s2 = new SampleClass();
            //s2.x = 55;   // OK
            //Console.WriteLine("p2: x={0}, y={1}, z={2}", s2.x, s2.y, s2.z);
            #endregion

            #region Ref-Out
            //RefOutWithParameters.OutTryGet();
            //RefOutWithParameters.RefValueType();
            //RefOutWithParameters.RefReferenceType();
            //RefOutWithParameters.Resize();
            #endregion

            //StringCSharp str = new StringCSharp();

            // String interpolation
            //str.StringInterpolation();

            // String with format standard
            //str.StringWithFormatStandard();

            // Compare 
            //str.CompareStringAndStringBuilder();
            //str.StringWithCommonMethod();


            #region Equal
            //var obj1 = new Equal();
            //obj1.Name = "Huy Nguyen Gia";
            //obj1.Age = 22;

            //var obj2 = new Equal();
            //obj2.Name = "Huy Nguyen Gia";
            //obj2.Age = 22;

            //Console.WriteLine(obj1 == obj2);
            //Console.WriteLine(obj1.Equals(obj2));

            //var obj = new Equal();
            //obj.Compare();

            #endregion


            #region Abstract
            // abstract 
            //AbstractJob job1 = new JavaCoding();
            //job1.DoJob();
            //string jobName1 = job1.GetJobName();
            //Console.WriteLine("Job Name 1= " + jobName1);

            //AbstractJob job2 = new CSharpCoding();
            //job2.DoJob();
            //string jobName2 = job2.GetJobName();
            //Console.WriteLine("Job Name 2= " + jobName2);

            //AbstractJob job = new CSharpCoding();
            //job.DoSomething();
            //string jobName= job2.GetJobName();
            //Console.WriteLine("Job Name 2= " + jobName);

            //AbstractJob job3 = new BuildHouse();
            //job3.DoJob();
            //String jobName3 = job3.GetJobName();
            //Console.WriteLine("Job Name 3= " + jobName3);
            #endregion

            #region Using & Dispose
            //UsingAndDispose usi = new UsingAndDispose();
            //usi.UseUsing();

            //using (var myDisposable = new MyDisposable())
            //{
            //    myDisposable.DoSomething();
            //}
            #endregion
        }
    }
}
