using TourBooking.ApiService.Contracts;
using TourBooking.ServiceDefaults;
using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;
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

var toursGroup = app.MapGroup("/tours");

toursGroup.MapPost("/", async (CreateTourRequest request, IToursStore store, IUnitOfWork uow, CancellationToken ct) =>
{
    var tour = new Tour(request.Name, request.Description, request.Price, request.StartDate, request.EndDate);

    store.Add(tour);

    await uow.SaveChanges(ct);

    return TypedResults.Ok();
})
.WithName("CreateTour");

toursGroup.MapGet("/", async (IToursStore store, CancellationToken ct) =>
{
    var tours = await store.GetAll(ct);

    return TypedResults.Ok(tours);
})
.WithName("GetAllTours");

app.MapDefaultEndpoints();

await app.RunAsync();