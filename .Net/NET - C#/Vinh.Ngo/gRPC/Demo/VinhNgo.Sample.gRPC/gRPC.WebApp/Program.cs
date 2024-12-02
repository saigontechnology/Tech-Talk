using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using gRPC.WebApp.Data;
using gRPC.WebApp.Interceptors;
using gRPC.WebApp.Services;
using VinhNgo.Sample.gRPC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddSingleton<ClientLoggingInterceptor>();

builder.Services.AddGrpcClient<User.UserClient>(options =>
{
    options.Address = new Uri("https://localhost:7230");
})
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()))
    .AddInterceptor<ClientLoggingInterceptor>();

builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();