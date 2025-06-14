var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithRedisCommander();

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb();

var database = postgres.AddDatabase("tourbooking");

var migrationService = builder.AddProject<Projects.TourBooking_MigrationService>("migrationservice")
    .WithReference(database)
    .WaitFor(database);

var apiService = builder.AddProject<Projects.TourBooking_ApiService>("apiservice")
    .WaitFor(redis)
    .WithReference(database)
    .WaitFor(database)
    .WaitForCompletion(migrationService);

builder.AddProject<Projects.TourBooking_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WaitFor(database)
    .WithReference(apiService)
    .WaitFor(apiService);

await builder.Build().RunAsync();
