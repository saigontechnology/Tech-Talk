using Microsoft.OpenApi.Models;
using ProtoBuf.Grpc.Server;
using VinhNgo.gRPC.Shared.Contracts;
using VinhNgo.Sample.gRPC;
using VinhNgo.Sample.gRPC.Interceptors;
using VinhNgo.Sample.gRPC.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// 
builder.Services.ConfigureAuthServices(builder.Configuration);
// builder.Services.AddCodeFirstGrpc();
builder.Services.AddSingleton<ServerLoggingInterceptor>();
builder.Services.AddSingleton<ErrorHandlingInterceptor>();

builder.Services
    .AddMySql<MainDBContext>(builder.Configuration)
    .AddGrpc(options =>
    {
        options.Interceptors.Add<ServerLoggingInterceptor>();
        options.Interceptors.Add<ErrorHandlingInterceptor>();
    })
    .AddJsonTranscoding(); // add Microsoft.AspNetCore.Grpc.JsonTranscoding

// add Microsoft.AspNetCore.Grpc.Swagger
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "gRPC swagger", Version = "v1" });
});

builder.Services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    builder
        .WithOrigins("http://example.com") // Replace with your allowed origin
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseRouting();
app.UseGrpcWeb();

app.UseAuthentication();
app.UseAuthorization();

// app.UseMiddleware<ApiKeyMiddleware>();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
// app.MapGrpcService<IToDoService>();
// app.MapGrpcService<UserService>();
app.UseCors(MyAllowSpecificOrigins);

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserService>().EnableGrpcWeb().RequireCors(MyAllowSpecificOrigins);
});

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateDatabase<MainDBContext>();

app.Run();