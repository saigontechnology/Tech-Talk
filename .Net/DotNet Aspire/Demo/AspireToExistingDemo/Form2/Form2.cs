using Microsoft.Extensions.DependencyInjection;
using SharedDomains;
using System.Net.Http.Json;

namespace Form2;

public partial class Form2 : Form
{
    private readonly HttpClient _storeApi;
    protected StoreModel[] _stores;

    public Form2()
    {
        InitializeComponent();

        var httpFactory = Services.GetRequiredService<IHttpClientFactory>();
        _storeApi = httpFactory.CreateClient(DomainConst.HTTP_CLIENT_STORE);

        _stores = [];

        btnLoad.Click += BtnLoad_Click;
    }

    private async void BtnLoad_Click(object? sender, EventArgs e)
    {
        _stores = await _storeApi.GetFromJsonAsync<StoreModel[]>("/store") ?? [];

        gridStore.DataSource = _stores;
    }

    protected override void OnClosed(EventArgs e)
    {
        _storeApi.Dispose();

        base.OnClosed(e);
    }
}
