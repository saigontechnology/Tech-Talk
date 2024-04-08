using efcore_performances;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace efcore_performances;
public class Runner
{
    private static Dictionary<string, Runner> Instances = new();
    private const string Default_Identify = "Main_Runner";
    private bool _disposed = false;
    private string _identifier;

    private Runner(string identifier)
    {
        _identifier = identifier;
    }

    public static Runner GetInstance(string identify = null)
    {
        if (string.IsNullOrEmpty(identify))
        {
            identify = Default_Identify;
        }

        Instances.TryGetValue(identify, out var instance);

        if (instance is null)
        {
            instance = new Runner(identify);
            Instances.TryAdd(identify, instance);
        }
        return instance;
    }

    public void Build(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var exapleType = typeof(IExample);
        IList<Type> allExampleTypes = GetImplementations(exapleType);

        foreach (var type in allExampleTypes)
        {
            serviceCollection.AddScoped(exapleType, type);
        }
    }

    private List<Type> GetImplementations(Type targetType)
    {
        var assembly = Assembly.GetEntryAssembly();
        var allExamples = assembly
            .GetTypes()
            .Where(t => t.IsInterface == false && t.IsAssignableTo(targetType))
            .ToList();

        return allExamples;
    }

    private readonly static List<string> exitSigns = new List<string> { "0", "exit" };

    public void Execute()
    {
        ExecuteAsync().Wait();
    }
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var container = Container.GetInstance();
        do
        {
            Console.Clear();
            using var scoped = container.ServiceProvider.CreateScope();
            var examples = scoped.ServiceProvider.GetServices<IExample>().ToList();

            if (examples is null || examples.Count == 0)
            {
                Console.WriteLine("No function found.");

                return;
            }

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

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
    }
}