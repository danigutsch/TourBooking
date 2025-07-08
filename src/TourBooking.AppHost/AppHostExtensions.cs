using TourBooking.AppHost.Grafana;
using TourBooking.AppHost.Jaeger;
using TourBooking.AppHost.OpenTelemetryCollector;
using TourBooking.AppHost.Prometheus;
using CommunityToolkit.Diagnostics;
using TourBooking.Constants;

namespace TourBooking.AppHost;

/// <summary>
/// Provides extension methods for configuring distributed application resources, including observability tools,
/// databases, and other infrastructure components.
/// </summary>
/// <remarks>This class contains methods to simplify the setup of common resources such as Prometheus, Grafana,
/// Jaeger,  OpenTelemetry Collector, Redis, and PostgreSQL within a distributed application. These methods are designed
/// to streamline resource configuration and ensure proper integration between components.</remarks>
public static class AppHostExtensions
{
    public static IResourceBuilder<OpenTelemetryCollectorResource> AddObservability(this IDistributedApplicationBuilder builder)
    {

        var prometheus = builder.AddPrometheus();

        var grafana = builder.AddGrafana(prometheus);

        var jaeger = builder.AddJaeger();

        return builder.AddOpenTelemetryCollector(prometheus, grafana, jaeger);
    }

    public static IResourceBuilder<PrometheusResource> AddPrometheus(this IDistributedApplicationBuilder builder)
    {
        Guard.IsNotNull(builder);

        return builder.AddPrometheus(ResourceNames.Prometheus, "../../../prometheus")
            .WithContainerName(ResourceNames.Prometheus)
            .WithLifetime(ContainerLifetime.Persistent);
    }

    public static IResourceBuilder<GrafanaResource> AddGrafana(this IDistributedApplicationBuilder builder, IResourceBuilder<PrometheusResource> prometheus)
    {
        Guard.IsNotNull(builder);

        return builder.AddGrafana(
                name: ResourceNames.Grafana,
                configPath: "../../../grafana/config",
                dashboardsPath: "../../../grafana/dashboards")
            .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
            .WithContainerName(ResourceNames.Grafana)
            .WithLifetime(ContainerLifetime.Persistent);
    }

    public static IResourceBuilder<JaegerResource> AddJaeger(this IDistributedApplicationBuilder builder)
    {
        Guard.IsNotNull(builder);

        return builder.AddJaeger(ResourceNames.Jaeger)
            .WithContainerName(ResourceNames.Jaeger)
            .WithLifetime(ContainerLifetime.Persistent);
    }

    public static IResourceBuilder<OpenTelemetryCollectorResource> AddOpenTelemetryCollector(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<PrometheusResource> prometheus,
        IResourceBuilder<GrafanaResource> grafana,
        IResourceBuilder<JaegerResource> jaeger)
    {
        Guard.IsNotNull(builder);

        return builder.AddOpenTelemetryCollector(
                name: ResourceNames.OpenTelemetryCollector,
                configFileLocation: "../../../otelcollector/config.yaml")
            .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
            .WithEnvironment("JAEGER_ENDPOINT", jaeger.GetEndpoint("otlp-grpc"))
            .WithContainerName(ResourceNames.OpenTelemetryCollector)
            .WithLifetime(ContainerLifetime.Persistent)
            .WaitFor(prometheus)
            .WaitFor(grafana)
            .WaitFor(jaeger);
    }

    public static IResourceBuilder<RedisResource> AddRedis(this IDistributedApplicationBuilder builder)
    {
        Guard.IsNotNull(builder);

        var redis = builder.AddRedis(ResourceNames.Redis);

        if (Environment.GetEnvironmentVariable(EnvironmentVariables.AspireIncludeDevTools) is "true")
        {
            redis.WithRedisInsight()
                .WithRedisCommander();
        }

        return redis;
    }


    public static IResourceBuilder<PostgresServerResource> AddPostgres(this IDistributedApplicationBuilder builder)
    {
        Guard.IsNotNull(builder);

        var postgres = builder.AddPostgres(ResourceNames.PostgreSql);

        if (Environment.GetEnvironmentVariable(EnvironmentVariables.AspireIncludeDevTools) is "true")
        {
            postgres.WithPgWeb();
        }

        return postgres;
    }
}
