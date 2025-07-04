using JetBrains.Annotations;

namespace TourBooking.ApiService.Contracts;

/// <summary>
/// Represents the data transfer object for retrieving information about a tour.
/// </summary>
[PublicAPI]
public sealed record GetTourDto(
    string Name,
    string Description,
    decimal Price,
    DateOnly StartDate,
    DateOnly EndDate);
