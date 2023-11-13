

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using var channel = new InMemoryChannel();

try
{
    IServiceCollection services = new ServiceCollection();
    services.Configure<TelemetryConfiguration>(x => x.TelemetryChannel = channel);
    services.AddLogging(x =>
    {
        x.AddApplicationInsights(
            configureTelemetryConfiguration: teleConfig =>
                teleConfig.ConnectionString =
                    "InstrumentationKey=3f0c4b53-89b6-4b84-811c-5659284c9071;IngestionEndpoint=https://northeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://northeurope.livediagnostics.monitor.azure.com/",
            configureApplicationInsightsLoggerOptions: _ => { });
    });

    var serviceProvider = services.BuildServiceProvider();

    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("Hello from console!");

}
finally
{
    await channel.FlushAsync(default);
    await Task.Delay(1000);
}
