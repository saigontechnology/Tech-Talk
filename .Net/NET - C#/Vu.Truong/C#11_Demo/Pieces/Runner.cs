namespace C_11_Demo.Pieces;
internal class Runner
{
    private static List<Type> GetImplementations(Type targetType)
    {
        var allExamples = Assembly.GetExecutingAssembly()
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
            var allExampleTypes = GetImplementations(typeof(IExample));

            if (allExampleTypes.IsNullOrEmpty())
            {
                Console.WriteLine("No demo found.");

                return;
            }

            Console.Clear();
            Console.WriteLine("Pick a demo by number: (enter 0 or exit to cancel)");
            var allExamples = allExampleTypes
                .Select(type =>
                {
                    var order = type.GetProperty(nameof(IExample.Order))?.GetValue(null, null) as int?;
                    var name = type.GetProperty(nameof(IExample.Name))?.GetValue(null, null) as string;

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
                var demoType = allExamples[choosen];

                Console.WriteLine("------------------------");
                Console.WriteLine($"Running {demoType.Name}...");
                Console.WriteLine("------------------------");
                Console.WriteLine();

                demoType.Type.GetMethod(nameof(IExample.Execute))?.Invoke(null, null);

                Console.WriteLine();
                Console.WriteLine("------------------------");
                Console.WriteLine($"{demoType.Name} end.");
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
