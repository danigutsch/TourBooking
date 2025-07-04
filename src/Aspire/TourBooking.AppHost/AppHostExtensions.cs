using TourBooking.AppHost.Grafana;
using TourBooking.AppHost.Jaeger;
using TourBooking.AppHost.OpenTelemetryCollector;
using TourBooking.AppHost.Prometheus;
using TourBooking.Aspire.Constants;

namespace TourBooking.AppHost;

internal static class AppHostExtensions
{
    public static IResourceBuilder<OpenTelemetryCollectorResource> AddObservability(this IDistributedApplicationBuilder builder)
    {
        var prometheus = builder.AddPrometheus();

        var grafana = builder.AddGrafana(prometheus);

        var jaeger = builder.AddJaeger();

        return builder.AddOpenTelemetryCollector(prometheus, grafana, jaeger);
    }

    public static IResourceBuilder<PrometheusResource> AddPrometheus(this IDistributedApplicationBuilder builder) =>
        builder.AddPrometheus(ResourceNames.Prometheus, "../../../prometheus")
            .WithContainerName(ResourceNames.Prometheus)
            .WithLifetime(ContainerLifetime.Persistent);

    public static IResourceBuilder<GrafanaResource> AddGrafana(this IDistributedApplicationBuilder builder, IResourceBuilder<PrometheusResource> prometheus) =>
        builder.AddGrafana(
                name: ResourceNames.Grafana,
                configPath: "../../../grafana/config",
                dashboardsPath: "../../../grafana/dashboards")
            .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
            .WithContainerName(ResourceNames.Grafana)
            .WithLifetime(ContainerLifetime.Persistent);

    public static IResourceBuilder<JaegerResource> AddJaeger(this IDistributedApplicationBuilder builder) =>
        builder.AddJaeger(ResourceNames.Jaeger)
            .WithContainerName(ResourceNames.Jaeger)
            .WithLifetime(ContainerLifetime.Persistent);

    public static IResourceBuilder<OpenTelemetryCollectorResource> AddOpenTelemetryCollector(this IDistributedApplicationBuilder builder, IResourceBuilder<PrometheusResource> prometheus, IResourceBuilder<GrafanaResource> grafana, IResourceBuilder<JaegerResource> jaeger) =>
        builder.AddOpenTelemetryCollector(
                name: ResourceNames.OpenTelemetryCollector,
                configFileLocation: "../../../otelcollector/config.yaml")
            .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
            .WithEnvironment("JAEGER_ENDPOINT", jaeger.GetEndpoint("otlp-grpc"))
            .WithContainerName(ResourceNames.OpenTelemetryCollector)
            .WithLifetime(ContainerLifetime.Persistent)
            .WaitFor(prometheus)
            .WaitFor(grafana)
            .WaitFor(jaeger);

    public static IResourceBuilder<RedisResource> AddRedis(this IDistributedApplicationBuilder builder)
    {
        var redis = builder.AddRedis(ResourceNames.Redis);

#if DEBUG
        redis.WithRedisInsight()
            .WithRedisCommander();
#endif

        return redis;
    }


    public static IResourceBuilder<PostgresServerResource> AddPostgres(this IDistributedApplicationBuilder builder)
    {
        var postgres = builder.AddPostgres(ResourceNames.PostgreSql);

#if DEBUG
        postgres.WithPgWeb();
#endif

        return postgres;
    }

}
