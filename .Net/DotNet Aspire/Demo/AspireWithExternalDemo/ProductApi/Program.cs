using OpenTelemetry.Resources;
using SharedDomains;

var builder = WebApplication.CreateBuilder(args);

var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
var isSelfHosted = !string.IsNullOrWhiteSpace(builder.Configuration["SELF_HOSTED"]);

// Add services to the container.

if (isSelfHosted)
{
    builder.AddOpenTelemetryConfigure(
        c => c.AddService(DomainConst.EndpointConst.API_PRODUCT),
        ["product"]
    );
}

builder.AddServiceDefaults();
builder.AddRedisOutputCache(DomainConst.EndpointConst.SERVICE_REDIS);

builder.Services.AddControllers();
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

app.MapDefaultEndpoints();
app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
