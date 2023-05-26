using System;
using System.Threading;

namespace SimulateMemoryUsageTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Arg 0: {args[0]}");

            long numOfArrays = long.Parse(args[0]);

            byte[][] memArray = new byte[numOfArrays][];
            for (int i = 0; i < memArray.Length; i++)
            {
                memArray[i] = new byte[1024 * 1024];
            }

            for (int i = 0; i < memArray.Length; i++)
            {
                for (int x = 0; x < memArray[i].Length; x++)
                {
                    memArray[i][x] = (byte)x;
                }
            }

            while (true)
            {
                Console.WriteLine(memArray.Length);
                Thread.Sleep(7000);
            }
        }
    }
}
