namespace TourBooking.ApiService.Contracts;

/// <summary>
/// Represents the data required to create a new tour.
/// </summary>
public sealed record CreateTourDto(
    string Name,
    string Description,
    decimal Price,
    DateOnly StartDate,
    DateOnly EndDate);
