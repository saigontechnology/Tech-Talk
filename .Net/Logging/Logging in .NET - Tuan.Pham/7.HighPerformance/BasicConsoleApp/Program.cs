using BasicConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.AddConsole();
    })
    .ConfigureServices(x =>
    {
        x.AddSingleton<PaymentService>();
    })
    .Build();

var example = host.Services.GetService<PaymentService>()!;

example.CreatePayment("tuan@saigon.com", 15.99m, 1);




