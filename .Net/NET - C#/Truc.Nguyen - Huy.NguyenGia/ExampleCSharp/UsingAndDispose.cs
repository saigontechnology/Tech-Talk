using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExampleCSharp
{
    public class UsingAndDispose
    {
        public void UseUsing()
        {
            using (var streamReader = new StreamReader(@"C:\Users\huy.nguyengia\Desktop\C#Advance.txt"))
            {
                Console.Write(streamReader.ReadToEnd());
            }
        }

        public void NoUsing()
        {
            {
                var streamReader = new StreamReader(@"C:\Users\huy.nguyengia\Desktop\C#Advance.txt");
                try
                {
                    Console.Write(streamReader.ReadToEnd());
                }
                finally
                {
                    if (streamReader != null)
                        streamReader.Dispose();
                }
            }
        }
    }

    public class MyDisposable : IDisposable
    {
        public MyDisposable() { Console.WriteLine("Allocating resources"); }
        public void DoSomething() { Console.WriteLine("Using resources"); }
        public void Dispose() { Console.WriteLine("Releasing resources"); }
    }
}
