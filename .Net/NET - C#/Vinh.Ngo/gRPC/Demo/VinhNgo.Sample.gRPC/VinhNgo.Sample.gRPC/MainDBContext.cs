using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VinhNgo.Sample.gRPC.Entities;

namespace VinhNgo.Sample.gRPC;

public class MainDBContext(DbContextOptions<MainDBContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEntity>(user => user.HasKey(t => t.Id));
    }
}

public static class Startup
{
    public static IServiceCollection AddMySql<T>(this IServiceCollection services, IConfiguration config)
        where T : DbContext
    {
        var connectionString = config.GetConnectionString("Default")!;

        services.AddDbContext<T>((sp, m) =>
        {
            m.UseMySQL(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName));
        });

        services.AddScoped<T>();

        return services;
    }
    
    public static async Task MigrateDatabase<T>(this IApplicationBuilder application) where T : DbContext
    {
        using var scoped = application.ApplicationServices.CreateScope();
        await using var dbContext = scoped.ServiceProvider.GetRequiredService<T>();

        await dbContext.Database.MigrateAsync();
    }
    
    public static void ConfigureAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
            var tokenValidatorParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes("TMZJC9SZZXj2ytcs5m2b37nvh3trrhbz9ib7ncwitcv"))
            };

        services.AddSingleton(tokenValidatorParams);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = tokenValidatorParams;

                // Handle authentication failures
                option.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization();
    }
}