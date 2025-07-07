using TourBooking.ApiService.Contracts;
using TourBooking.ApiService.Contracts.Models;
using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;

namespace TourBooking.ApiService.Endpoints;

/// <summary>
/// Defines the Tours API endpoints.
/// </summary>
internal static class ToursEndpoints
{
    /// <summary>
    /// Maps the Tours API endpoints to the application.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <returns>The configured route group builder.</returns>
    public static RouteGroupBuilder MapToursApi(this WebApplication app)
    {
        var toursGroup = app.MapGroup(ToursApiEndpoints.ToursBasePath)
            .WithTags("Tours");

        return toursGroup.MapToursEndpoints();
    }

    /// <summary>
    /// Maps the Tours API endpoints to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <returns>The configured route group builder.</returns>
    private static RouteGroupBuilder MapToursEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateTour)
            .WithName("CreateTour")
            .WithSummary("Create a new tour")
            .WithDescription("Creates a new bike tour with the specified details. The Location header will contain the URI of the created tour.")
            .Produces<GetTourDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithTags("Tours");

        group.MapGet("/", GetAllTours)
            .WithName("GetAllTours")
            .WithSummary("Get all tours")
            .WithDescription("Retrieves all available bike tours")
            .Produces<IEnumerable<GetTourDto>>()
            .WithTags("Tours");

        return group;
    }

    /// <summary>
    /// Creates a new tour.
    /// </summary>
    /// <param name="dto">The tour creation data.</param>
    /// <param name="store">The tours store.</param>
    /// <param name="uow">The unit of work.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created tour with a 201 Created status. The Location header contains the URI of the created resource.</returns>
    private static async Task<IResult> CreateTour(
        CreateTourDto dto,
        IToursStore store,
        IUnitOfWork uow,
        CancellationToken ct)
    {
        var tour = new Tour(dto.Name, dto.Description, dto.Price, dto.StartDate, dto.EndDate);

        store.Add(tour);
        await uow.SaveChanges(ct);

        var responseDto = new GetTourDto(tour.Name, tour.Description, tour.Price, tour.StartDate, tour.EndDate);
        return TypedResults.Created($"/tours/{tour.Id}", responseDto);
    }

    /// <summary>
    /// Gets all available tours.
    /// </summary>
    /// <param name="store">The tours store.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>All available tours.</returns>
    private static async Task<IResult> GetAllTours(
        IToursStore store,
        CancellationToken ct)
    {
        var tours = await store.GetAll(ct);
        var tourDtos = tours.Select(tour => new GetTourDto(tour.Name, tour.Description, tour.Price, tour.StartDate, tour.EndDate));
        return TypedResults.Ok(tourDtos);
    }
}
