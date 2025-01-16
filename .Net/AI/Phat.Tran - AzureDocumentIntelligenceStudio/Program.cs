using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Identity;
using AzureDocumentIntelligenceStudio.Helpers;
using AzureDocumentIntelligenceStudio.Models.Utils;
using AzureDocumentIntelligenceStudio.Services;
using Microsoft.Extensions.Options;
using Azure.AI.OpenAI;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add configures
builder.Services.Configure<AzureDocumentIntelligenceConfig>(
    builder.Configuration.GetRequiredSection("AzureDocumentIntelligence"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureDocumentIntelligenceConfig>>().Value;
    return new DocumentIntelligenceClient(
        new Uri(options.EndPoint),
        new AzureKeyCredential(options.ApiKey));
});

builder.Services.Configure<AzureOpenAIConfig>(builder.Configuration.GetRequiredSection("AzureOpenAI"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIConfig>>().Value;
    var azureOpAIClient = new AzureOpenAIClient(new Uri(options.EndPoint), new AzureKeyCredential(options.ApiKey));
    var chatClient = azureOpAIClient.GetChatClient(options.ModelName);
    return chatClient;
});

builder.Services.AddSingleton<IFileHelper, FileHelper>();
builder.Services.AddSingleton<IResumeHelpers, ResumeHelpers>();
builder.Services.AddScoped<IAzureDocumentIntelligenceService, AzureDocumentIntelligenceService>();
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
