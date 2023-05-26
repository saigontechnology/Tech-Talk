using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo.Options;
using HangfireDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailServerSettings>(
    builder.Configuration.GetSection(EmailServerSettings.EmailServer));
builder.Services.Configure<DefaultEmailSettings>(DefaultEmailSettings.Sender,
    builder.Configuration.GetSection("DefaultEmail:Sender"));
builder.Services.Configure<DefaultEmailSettings>(DefaultEmailSettings.Recipient,
    builder.Configuration.GetSection("DefaultEmail:Recipient"));

builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddSingleton<BackgroundJobService>();

var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

//using (var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;

//    var service = services.GetRequiredService<BackgroundJobService>();
//    service.RegisterRecurringJobs();
//}

app.MapControllers();

app.Run();
