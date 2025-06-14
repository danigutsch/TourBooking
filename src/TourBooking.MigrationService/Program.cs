using TourBooking.MigrationService;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.AddToursPersistenceServices();

builder.AddServiceDefaults();
builder.Services.AddHostedService<Migrator>();

var host = builder.Build();
await host.RunAsync();
