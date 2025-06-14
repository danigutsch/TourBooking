var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithRedisCommander();

var apiService = builder.AddProject<Projects.TourBooking_ApiService>("apiservice");
    .WaitFor(redis)

builder.AddProject<Projects.TourBooking_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(apiService)
    .WaitFor(apiService);

await builder.Build().RunAsync();
