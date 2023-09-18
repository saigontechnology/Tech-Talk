using System.Reflection;
using System.Text;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Endpoints;
using ProductCatalog.Api.Middlewares;
using ProductCatalog.Api.OpenApi;
using ProductCatalog.Api.Repository;
using ProductCatalog.Api.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

#region ConfigureServices
var builder = WebApplication.CreateBuilder(args);

// Add environments
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", false, true);

// Add services to the container.
builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection"));
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())); // console sql script
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();

// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add Validator
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add Repositories
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));

// Add Services
builder.Services.AddScoped(typeof(IJwtService), typeof(JwtService));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<SwaggerDefaultValues>();

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer <token>')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add API Versioning
builder.Services.AddApiVersioning(option =>
{
    option.DefaultApiVersion = new ApiVersion(1);
    option.AssumeDefaultVersionWhenUnspecified = true;
    //option.ApiVersionReader = new HeaderApiVersionReader("api-version");
    //option.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
    //option.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    option.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV";
    option.SubstituteApiVersionInUrl = true;
});

#endregion

#region Configure

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

#endregion

// Use API Versioning
var versionSet = app.NewApiVersionSet()
    .HasApiVersion(1)
    .HasApiVersion(2)
    .ReportApiVersions()
    .Build();

// Map Endpoints
app.MapGroup("v{version:apiVersion}/users")
    .MapUserGroup()
    .WithTags("Users")
    .WithOpenApi()
    .WithApiVersionSet(versionSet);

app.MapGroup("v{version:apiVersion}/categories")
    .MapCategoryGroup()
    .WithTags("Categories")
    .WithOpenApi()
    .RequireAuthorization()
    .WithApiVersionSet(versionSet);

app.MapGroup("v{version:apiVersion}/products")
    .MapProductGroup()
    .WithTags("Products")
    .WithOpenApi()
    .RequireAuthorization()
    .WithApiVersionSet(versionSet);

if (app.Environment.IsEnvironment("Debug") || app.Environment.IsEnvironment("Development"))
{
    app.UseSwagger();

    app.UseSwaggerUI(option =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"ProductCatalog API - {description.GroupName.ToUpper()}");
        }
    });
}

app.Run();