using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace TourBooking.ServiceDefaults;

/// <summary>
/// Provides extension methods to add common .NET Aspire services such as service discovery, resilience, health checks, and OpenTelemetry.
/// This static class should be referenced by each service project in your solution.
/// </summary>
[PublicAPI]
public static class ServiceDefaultExtensions
{
    /// <summary>
    /// Adds default service configuration including OpenTelemetry, health checks, service discovery, and HTTP client defaults.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder instance.</param>
    /// <returns>The configured host application builder.</returns>
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
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

    /// <summary>
    /// Configures OpenTelemetry logging, metrics, and tracing for the application.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder instance.</param>
    /// <returns>The configured host application builder.</returns>
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddNpgsqlInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Adds OpenTelemetry exporters if the OTLP endpoint is configured.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder instance.</param>
    /// <returns>The configured host application builder.</returns>
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }

    /// <summary>
    /// Adds default health checks to the application, including a liveness check.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder instance.</param>
    /// <returns>The configured host application builder.</returns>
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    /// <summary>
    /// Maps default health check endpoints for the application in development environments.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application with mapped endpoints.</returns>
    [MustDisposeResource(false)]
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }
}
