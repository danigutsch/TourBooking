using TourBooking.ApiService.Endpoints;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddToursPersistenceServices();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapToursApi();

app.MapDefaultEndpoints();

await app.RunAsync();