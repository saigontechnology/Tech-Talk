using Dumpify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace efcore_demos;
public class Runner
{
    public static IServiceProvider ServiceProvider { get; set; }

    public static void Build(Action<IServiceCollection, IConfiguration> startupAction = null)
    {
        var serviceCollection = new ServiceCollection()
            .AddLogging();

        var exapleType = typeof(IExample);
        IList<Type> allExampleTypes = GetImplementations(exapleType);

        foreach (var type in allExampleTypes)
        {
            serviceCollection.AddScoped(exapleType, type);
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        serviceCollection.AddSingleton<IConfiguration>(configuration);

        if (startupAction is not null)
        {
            startupAction(serviceCollection, configuration);
        }

        ServiceProvider = serviceCollection
                .BuildServiceProvider();
    }

    private static List<Type> GetImplementations(Type targetType)
    {
        var assembly = Assembly.GetEntryAssembly();
        var allExamples = assembly
            .GetTypes()
            .Where(t => t.IsInterface == false && t.IsAssignableTo(targetType))
            .ToList();

        return allExamples;
    }

    private readonly static List<string> exitSigns = new List<string> { "0", "exit" };

    public static void Execute()
    {
        ExecuteAsync().Wait();
    }
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        do
        {
            using var scoped = ServiceProvider.CreateScope();
            var examples = scoped.ServiceProvider.GetServices<IExample>().ToList();

            if (examples is null || examples.Count == 0)
            {
                Console.WriteLine("No function found.");

                return;
            }

            //Console.Clear();
            var allExamples = examples
                .Select(type => new
                {
                    type.GetType().Name,
                    Type = type
                })
                .OrderBy(x => x.Name)
                .Select((type, index) =>
                {
                    var name = type.Name;

                    return new
                    {
                        Order = index + 1,
                        Name = name,
                        Instance = type.Type
                    };
                })
                .Where(x => x.Order > 0)
                .OrderBy(x => x.Order)
                .ToDictionary(x => x.Order.ToString(), x => x);

            foreach (var example in allExamples)
            {
                Console.WriteLine($"{example.Value.Order}. {example.Value.Name}");
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Pick a function by number: (enter 0 or exit to cancel)");

            var chosen = (Console.ReadLine() ?? "1").Trim().Replace(" ", "").ToLower();
            Console.Clear();

            if (allExamples.TryGetValue(chosen, out var choosen))
            {
                Console.WriteLine("------------------------");
                Console.WriteLine($"Running {choosen.Name}...");
                Console.WriteLine("------------------------");
                Console.WriteLine();

                try
                {
                    await choosen.Instance.ExecuteAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    ex.Dump("Error during executing function");
                }

                Console.WriteLine();
                Console.WriteLine("------------------------");
                Console.WriteLine($"{choosen.Name} end.");
                Console.WriteLine("------------------------");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            if (exitSigns.Contains(chosen))
            {
                return;
            }
        }
        while (true);
    }
}