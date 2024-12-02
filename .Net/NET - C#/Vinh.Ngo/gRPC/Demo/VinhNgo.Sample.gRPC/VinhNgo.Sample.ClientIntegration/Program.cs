using Sts.Sample.Grpc.ClientIntegration.Services;
using VinhNgo.gRPC.Shared.Contracts;
using VinhNgo.Sample.gRPC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<Greeter.GreeterClient>(o => { o.Address = new Uri("https://localhost:7230"); });
// builder.Services.AddGrpcClient<IToDoService>(o => { o.Address = new Uri("https://localhost:7230"); });
    // .EnableCallContextPropagation();

builder.Services.AddScoped<TestService>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();