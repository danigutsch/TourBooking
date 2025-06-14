using TourBooking.MigrationService;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.AddToursMigrationManager();

builder.AddServiceDefaults();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Migrator.ActivitySourceName));

builder.Services.AddHostedService<Migrator>();

var host = builder.Build();
await host.RunAsync();
