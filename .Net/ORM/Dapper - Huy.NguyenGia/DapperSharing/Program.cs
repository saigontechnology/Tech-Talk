using DapperSharing.DapperExample;

namespace DapperSharing
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=========== GETTING STARTED ===========");
                var userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                              Exam01_QuickStart.Run();
                        break;
                    case "2":
                        await Exam02_QueryData.RunAsync();
                        break;
                    case "3":
                        await Exam03_MappingConfig.Run();
                        break;
                    case "4":
                        await Exam04_ExecuteNonQueryCommand.Run();
                        break;
                    case "5":
                        await Exam05_ExecuteReader.Run();
                        break;
                    case "6":
                        await Exam06_Relationships.Run();
                        break;
                    case "7":
                        await Exam07_Parameters.Run();
                        break;
                    case "8":
                        await Exam08_Transaction.Run();
                        break;
                    case "9":
                        await Exam09_Extensions.Run();
                        break;
                    //case "10":
                    //    BenchmarkRunner.Run<E10_DapperBenchmark>();
                    //    Console.ReadLine();
                    //    break;
                    //case "11":
                    //    BenchmarkRunner.Run<E11_EFBenchmark>();
                    //    Console.ReadLine();
                    //    break;
                    case "end":
                        return;
                }
            }
        }

        internal static partial class DBInfo
        {
            public const string ConnectionString = "Server=HUYNGUYENGIA;Initial Catalog=ShopOnline_Shoes;User ID=sa;Password=sa123;Trusted_Connection=False;Encrypt=False";
        }
    }
}