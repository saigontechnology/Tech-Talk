// See https://aka.ms/new-console-template for more information
using SharedDomains;
using System.Net.Http.Json;
using Dumpify;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.AddAppDefaults();

builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_STORE, client => client.BaseAddress = new($"https+http://{DomainConst.EndpointConst.API_STORE}"));
builder.Services.AddLogging();

var app = builder.Build();

await app.StartAsync();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

int fetchingCount = 0;
const int maxFetching = 10;

try
{
    while(true)
    {
        var httpFactory = app.Services.GetRequiredService<IHttpClientFactory>();
        var storeApi = httpFactory.CreateClient(DomainConst.HTTP_CLIENT_STORE);

        var stores = await storeApi.GetFromJsonAsync<StoreModel[]>("/store") ?? [];

        stores.DumpConsole();

        fetchingCount++;

        if (fetchingCount >= maxFetching)
        {
            break;
        }

        await Task.Delay(5000);
    }
}
catch (Exception ex)
{
    logger.LogError(ex, ex.Message);
}
finally
{
    await app.StopAsync();
}


