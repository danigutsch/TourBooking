using TourBooking.ApiService.Contracts;
using TourBooking.ApiService.Endpoints;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

builder.WebHost.UseKestrelHttpsConfiguration();

builder.AddToursPersistenceServices();

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, ToursApiJsonContext.Default);
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapToursApi();

app.MapDefaultEndpoints();

await app.RunAsync();
