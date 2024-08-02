using Microsoft.AspNetCore.Mvc;
using SharedDomains;
using System.Diagnostics;
using System.Net.Http.Json;
using WebMVC.Models;

namespace WebMVC.Controllers;
public class HomeController(
    [FromKeyedServices(DomainConst.HTTP_CLIENT_PRODUCT)] HttpClient productApi,
    [FromKeyedServices(DomainConst.HTTP_CLIENT_STORE)] HttpClient storeApi,
    ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Store(CancellationToken cancellationToken)
    {
        var categoriesTask = productApi.GetFromJsonAsync<IdNameModel<ProductCategoryType>[]>("/product/categories", cancellationToken: cancellationToken);
        var storesTask = storeApi.GetFromJsonAsync<StoreModel[]>("/store", cancellationToken: cancellationToken);

        await Task.WhenAll(categoriesTask, storesTask);

        var categories = await categoriesTask;
        var stores = await storesTask;

        return View((stores, categories));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
