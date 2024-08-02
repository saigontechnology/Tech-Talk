using SharedDomains;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_PRODUCT,
    client => client.BaseAddress = new Uri($"https+http://{DomainConst.EndpointConst.API_PRODUCT}"));

builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_STORE,
    client => client.BaseAddress = new Uri($"https+http://{DomainConst.EndpointConst.API_STORE}"));

builder.Services.AddKeyedScoped(DomainConst.HTTP_CLIENT_PRODUCT, (sp, key) => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DomainConst.HTTP_CLIENT_PRODUCT));
builder.Services.AddKeyedScoped(DomainConst.HTTP_CLIENT_STORE, (sp, key) => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DomainConst.HTTP_CLIENT_STORE));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
