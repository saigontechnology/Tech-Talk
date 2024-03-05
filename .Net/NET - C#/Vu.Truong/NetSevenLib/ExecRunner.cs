using NetSevenLib.Contracts;
using System.Reflection;

namespace NetSevenLib;
public class ExecRunner
{
    private static List<Type> GetImplementations(Type targetType)
    {
        var assembly = Assembly.GetEntryAssembly();
        var allExamples = assembly
            .GetTypes()
            .Where(t => t.IsInterface == false && t.IsAssignableTo(targetType))
            .ToList();

        return allExamples;
    }

    private static List<string> exitSigns = new List<string> { "0", "exit" };

    public static void Execute()
    {
        do
        {
            var allExampleTypes = GetImplementations(typeof(IExecFunction));

            if (allExampleTypes.IsNullOrEmpty())
            {
                Console.WriteLine("No function found.");

                return;
            }

            //Console.Clear();
            Console.WriteLine("Pick a function by number: (enter 0 or exit to cancel)");
            var allExamples = allExampleTypes
                .Select(type =>
                {
                    var order = type.GetProperty(nameof(IExecFunction.Order))?.GetValue(null, null) as int?;
                    var name = type.GetProperty(nameof(IExecFunction.Name))?.GetValue(null, null) as string;

                    return new
                    {
                        Order = order ?? 0,
                        Name = name,
                        Type = type
                    };
                })
                .Where(x => x.Order > 0)
                .OrderBy(x => x.Order)
                .ToDictionary(x => x.Order.ToString(), x => x);

            foreach (var example in allExamples)
            {
                Console.WriteLine($"{example.Value.Order}. {example.Value.Name}");
            }

            var choosen = (Console.ReadLine() ?? "1").Trim().Replace(" ", "").ToLower();
            Console.Clear();

            if (allExamples.ContainsKey(choosen))
            {
                var functionType = allExamples[choosen];

                Console.WriteLine("------------------------");
                Console.WriteLine($"Running {functionType.Name}...");
                Console.WriteLine("------------------------");
                Console.WriteLine();

                functionType.Type.GetMethod(nameof(IExecFunction.Execute))?.Invoke(null, null);

                Console.WriteLine();
                Console.WriteLine("------------------------");
                Console.WriteLine($"{functionType.Name} end.");
                Console.WriteLine("------------------------");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            if (exitSigns.Contains(choosen))
            {
                return;
            }
        }
        while (true);
    }
}
