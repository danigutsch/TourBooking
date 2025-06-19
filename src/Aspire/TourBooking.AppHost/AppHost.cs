using TourBooking.AppHost.Grafana;
using TourBooking.AppHost.Jaeger;
using TourBooking.AppHost.OpenTelemetryCollector;
using TourBooking.AppHost.Prometheus;
using TourBooking.Aspire.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var prometheus = builder.AddPrometheus(ResourceNames.Prometheus, "../../../prometheus");

var grafana = builder.AddGrafana(
        name: ResourceNames.Grafana,
        configPath: "../../../grafana/config",
        dashboardsPath: "../../../grafana/dashboards")
    .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
    .WithContainerName(ResourceNames.Grafana)
    .WithLifetime(ContainerLifetime.Persistent);

var jaeger = builder.AddJaeger(ResourceNames.Jaeger)
    .WithContainerName(ResourceNames.Jaeger)
    .WithLifetime(ContainerLifetime.Persistent);

var otelCollector = builder.AddOpenTelemetryCollector(
        name: ResourceNames.OpenTelemetryCollector,
        configFileLocation: "../../../otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
    .WithEnvironment("JAEGER_ENDPOINT", jaeger.GetEndpoint("otlp-grpc"))
    .WithContainerName(ResourceNames.OpenTelemetryCollector)
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(prometheus)
    .WaitFor(grafana)
    .WaitFor(jaeger);

var redis = builder.AddRedis(ResourceNames.Redis)
    .WithContainerName(ResourceNames.Redis)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight(
        redisInsight => redisInsight.WithContainerName(ResourceNames.RedisInsight))
    .WithRedisCommander(
        redisCommander => redisCommander.WithContainerName(ResourceNames.RedisCommander))
    .WaitFor(otelCollector);

var postgres = builder.AddPostgres(ResourceNames.PostgreSql)
    .WithPgWeb(pgWeb => pgWeb.WithContainerName(ResourceNames.PgWeb))
    .WithContainerName(ResourceNames.PostgreSql)
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(otelCollector);

var database = postgres.AddDatabase(ResourceNames.ToursDatabase);

var migrationService = builder.AddProject<Projects.TourBooking_MigrationService>(ResourceNames.MigrationService)
    .WithReference(database)
    .WaitFor(database);

var apiService = builder.AddProject<Projects.TourBooking_ApiService>(ResourceNames.ApiService)
    .WaitFor(redis)
    .WithReference(database)
    .WaitFor(database)
    .WaitForCompletion(migrationService);

builder.AddProject<Projects.TourBooking_Web>(ResourceNames.WebFrontend)
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WaitFor(database)
    .WithReference(apiService)
    .WaitFor(apiService);

await builder.Build().RunAsync();
