using TourBooking.ApiService.Endpoints;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddToursPersistenceServices();

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

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