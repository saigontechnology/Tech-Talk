using Azure.AI.OpenAI;
using Azure;
using AzureOpenAIResearch.Utils;
using Microsoft.Extensions.Options;
using AzureOpenAIResearch.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<AzureOpenAIConfig>(builder.Configuration.GetRequiredSection("AzureOpenAI"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIConfig>>().Value;
    var azureOpAIClient = new AzureOpenAIClient(new Uri(options.EndPoint), new AzureKeyCredential(options.ApiKey));
    var chatClient = azureOpAIClient.GetChatClient(options.ModelName);
    return chatClient;
});

builder.Services.AddScoped<IAzureOpenAIService, AzureOpenAIService>();
// Add services to the container.

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
