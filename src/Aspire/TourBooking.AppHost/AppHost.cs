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
    .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"));

var jaeger = builder.AddJaeger(ResourceNames.Jaeger);

var otelCollector = builder
    .AddOpenTelemetryCollector(ResourceNames.OpenTelemetryCollector, "../../../otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
    .WithEnvironment("JAEGER_ENDPOINT", jaeger.GetEndpoint("otlp-grpc"))
    .WaitFor(prometheus)
    .WaitFor(grafana)
    .WaitFor(jaeger);

var redis = builder.AddRedis(ResourceNames.Redis)
    .WithRedisInsight()
    .WithRedisCommander()
    .WaitFor(otelCollector);

var postgres = builder.AddPostgres(ResourceNames.PostgreSql)
    .WithPgWeb()
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
