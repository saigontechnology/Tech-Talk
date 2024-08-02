using SharedDomains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreContext") ?? throw new InvalidOperationException("Connection string 'StoreContext' not found.")));

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_PRODUCT,
    client => client.BaseAddress = new Uri($"https+http://{DomainConst.EndpointConst.API_PRODUCT}"));

builder.Services.AddKeyedScoped(DomainConst.HTTP_CLIENT_PRODUCT, (sp, key) => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DomainConst.HTTP_CLIENT_PRODUCT));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
