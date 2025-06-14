var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithRedisCommander();

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb();

var database = postgres.AddDatabase("tourbooking");

var apiService = builder.AddProject<Projects.TourBooking_ApiService>("apiservice")
    .WaitFor(redis)
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.TourBooking_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WaitFor(database)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.TourBooking_MigrationService>("tourbooking-migrationservice");

await builder.Build().RunAsync();
