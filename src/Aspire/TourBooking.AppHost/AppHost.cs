using TourBooking.AppHost;
using TourBooking.Aspire.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var prometheus = builder.AddPrometheus();

var grafana = builder.AddGrafana(prometheus);

var jaeger = builder.AddJaeger();

builder.AddOpenTelemetryCollector(prometheus, grafana, jaeger);

var redis = builder.AddRedis();

var postgres = builder.AddPostgres();

var database = postgres.AddDatabase(ResourceNames.ToursDatabase);

var migrationService = builder.AddProject<Projects.TourBooking_MigrationService>(ResourceNames.MigrationService)
    .WithReference(database)
    .WaitFor(database);

var apiService = builder.AddProject<Projects.TourBooking_ApiService>(ResourceNames.ApiService)
    .WithHttpHealthCheck("health")
    .WaitFor(redis)
    .WithReference(database)
    .WaitFor(database)
    .WaitForCompletion(migrationService);

builder.AddProject<Projects.TourBooking_Web>(ResourceNames.WebFrontend)
    .WithHttpHealthCheck("health")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WaitFor(database)
    .WithReference(apiService)
    .WaitFor(apiService);

await builder.Build().RunAsync();
