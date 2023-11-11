using BenchmarkDotNet.Running;
using Dapper;
using DapperSharing.Examples;
using DapperSharing.Models;
using DapperSharing.Utils;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace DapperSharing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=========== GETTING STARTED ===========");
                DisplayHelper.PrintAllClassNames(typeof(E01_QuickStart).Namespace);
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        E01_QuickStart.Run();
                        break;
                    case "2":
                        E02_QueryData.Run();
                        break;
                    case "3":
                        await E03_MappingConfig.Run();
                        break;
                    case "4":
                        await E04_ExecuteNonQueryCommand.Run();
                        break;
                    case "5":
                        await E05_ExecuteReader.Run();
                        break;
                    case "6":
                        await E06_Relationships.Run();
                        break;
                    case "7":
                        await E07_Parameters.Run();
                        break;
                    case "8":
                        await E08_Others.Run();
                        break;
                    case "9":
                        await E09_Extensions.Run();
                        break;
                    case "10":
                        BenchmarkRunner.Run<E10_DapperBenchmark>();
                        Console.ReadLine();
                        break;
                    case "11":
                        BenchmarkRunner.Run<E11_EFBenchmark>();
                        Console.ReadLine();
                        break;
                    case "end":
                        return;
                }
            }
        }

        internal static partial class DBInfo
        {
            public const string ConnectionString = "Server=DESKTOP-RESKER;Database=BikeStores;Trusted_Connection=True;Encrypt=False";
            //public const string ConnectionString = "Server=TUANPHAM;Database=BikeStores;Trusted_Connection=True;Encrypt=False";
        }
    }
}