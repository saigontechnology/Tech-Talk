using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting;

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
    static Action<ResourceBuilder> _configure = null;
    static string[] _tracingSources = [];
    public static IHostApplicationBuilder AddOpenTelemetryConfigure(this IHostApplicationBuilder builder, Action<ResourceBuilder> configure, string[] tracingSources)
    {
        _configure = configure;
        _tracingSources = tracingSources ?? [];

        return builder;
    }

    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        var openTelemetryBuilder = builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();

                if (_tracingSources is not null && _tracingSources.Length > 0)
                {
                    tracing.AddSource(_tracingSources);
                }
            });

        if (_configure is not null)
        {
            openTelemetryBuilder.ConfigureResource(_configure);
        }

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        var healthChecksConfiguration = builder.Configuration.GetSection("HealthChecks");

        // All health checks endpoints must return within the configured timeout value (defaults to 5 seconds)
        var healthChecksRequestTimeout = healthChecksConfiguration.GetValue<TimeSpan?>("RequestTimeout") ?? TimeSpan.FromSeconds(5);
        builder.Services.AddRequestTimeouts(timeouts => timeouts.AddPolicy("HealthChecks", healthChecksRequestTimeout));

        // Cache health checks responses for the configured duration (defaults to 10 seconds)
        var healthChecksExpireAfter = healthChecksConfiguration.GetValue<TimeSpan?>("ExpireAfter") ?? TimeSpan.FromSeconds(10);
        builder.Services.AddOutputCache(caching => caching.AddPolicy("HealthChecks", policy => policy.Expire(healthChecksExpireAfter)));

        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        var healthChecks = app.MapGroup("");

        // Configure health checks endpoints to use the configured request timeouts and cache policies
        healthChecks
            .CacheOutput(policyName: "HealthChecks")
            .WithRequestTimeout(policyName: "HealthChecks");

        // All health checks must pass for app to be considered ready to accept traffic after starting
        healthChecks.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        healthChecks.MapHealthChecks("/alive", new()
        {
            Predicate = r => r.Tags.Contains("live")
        });

        // Add the health checks endpoint for the HealthChecksUI
        var healthChecksUrls = app.Configuration["HEALTHCHECKSUI_URLS"];
        if (!string.IsNullOrWhiteSpace(healthChecksUrls))
        {
            var pathToHostsMap = GetPathToHostsMap(healthChecksUrls);

            foreach (var path in pathToHostsMap.Keys)
            {
                // Ensure that the HealthChecksUI endpoint is only accessible from configured hosts, e.g. localhost:12345, hub.docker.internal, etc.
                // as it contains more detailed information about the health of the app including the types of dependencies it has.

                healthChecks.MapHealthChecks(path, new() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
                    // This ensures that the HealthChecksUI endpoint is only accessible from the configured health checks URLs.
                    // See this documentation to learn more about restricting access to health checks endpoints via routing:
                    // https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0#use-health-checks-routing
                    .RequireHost(pathToHostsMap[path]);
            }
        }

        return app;
    }

    private static Dictionary<string, string[]> GetPathToHostsMap(string healthChecksUrls)
    {
        // Given a value like "localhost:12345/healthz;hub.docker.internal:12345/healthz" return a dictionary like:
        // { { "healthz", [ "localhost:12345", "hub.docker.internal:12345" ] } }

        var uris = healthChecksUrls.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(url => new Uri(url, UriKind.Absolute))
            .GroupBy(uri => uri.AbsolutePath, uri => uri.Authority)
            .ToDictionary(g => g.Key, g => g.ToArray());

        return uris;
    }
}
