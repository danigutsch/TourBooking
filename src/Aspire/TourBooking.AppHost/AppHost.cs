using TourBooking.AppHost.OpenTelemetryCollector;
using TourBooking.Aspire.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var prometheus = builder.AddContainer(ResourceNames.Prometheus, "prom/prometheus", "v3.2.1")
    .WithBindMount("../../../prometheus", "/etc/prometheus", isReadOnly: true)
    .WithArgs("--web.enable-otlp-receiver", "--config.file=/etc/prometheus/prometheus.yml")
    .WithHttpEndpoint(targetPort: 9090, name: "http");

var grafana = builder.AddContainer(ResourceNames.Grafana, "grafana/grafana")
    .WithBindMount("../../../grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../../../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
    .WithHttpEndpoint(targetPort: 3000, name: "http");

var otelCollector = builder.AddOpenTelemetryCollector(ResourceNames.OpenTelemetryCollector, "../../../otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
    .WaitFor(prometheus)
    .WaitFor(grafana);

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
