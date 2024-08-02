using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedDomains;

namespace Form2;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.AddAppDefaults();

        builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_STORE, client => client.BaseAddress = new($"https+http://{DomainConst.EndpointConst.API_STORE}"));

        var app = builder.Build();
        Services = app.Services;
        app.Start();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var mainForm = ActivatorUtilities.CreateInstance<Form2>(app.Services);
        Application.Run(mainForm);

        app.StopAsync().GetAwaiter().GetResult();
    }

    public static IServiceProvider Services { get; private set; } = default!;
}