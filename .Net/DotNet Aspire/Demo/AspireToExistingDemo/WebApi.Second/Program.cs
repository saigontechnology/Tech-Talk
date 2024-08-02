using Microsoft.EntityFrameworkCore;
using SharedDomains;
using WebApi.Second;
using WebAPI.Second;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(DomainConst.HTTP_CLIENT_STORE,
    client => client.BaseAddress = new Uri($"https+http://{DomainConst.EndpointConst.API_STORE}"));

builder.Services.AddKeyedScoped(DomainConst.HTTP_CLIENT_STORE, (sp, key) => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DomainConst.HTTP_CLIENT_STORE));

builder.AddSqlServerDbContext<ProductContext>(DomainConst.DB_PRODUCT);

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

await app.RunAsync();
