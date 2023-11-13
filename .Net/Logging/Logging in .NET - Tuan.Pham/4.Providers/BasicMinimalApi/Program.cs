using BasicMinimalApi;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Logging.ClearProviders();
    //builder.Logging.AddConsole();
    builder.Logging.AddProvider(new TuanLoggerProvider());
}
else
{
    builder.Logging.ClearProviders();
    builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: teleConfig =>
            teleConfig.ConnectionString = "InstrumentationKey=22c34b1a-2d91-49a7-b9fb-0a15f0aef24b;IngestionEndpoint=https://eastus-8.in." +
                                          "applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
        configureApplicationInsightsLoggerOptions: _ => { });
}

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    var name = "Tuan";
    logger.LogInformation("Hello from {name}!", name);
});

app.Run();
