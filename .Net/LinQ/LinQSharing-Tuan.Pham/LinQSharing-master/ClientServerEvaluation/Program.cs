using ClientServerEvaluation.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientServerEvaluation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MyMethods.TopLevelProjection();
            //MyMethods.UnsupportedClientEvaluation();
            //MyMethods.ExplicitClientEvaluation();
            MyMethods.InmemoryProcess();
        }
    }
}