using JetBrains.Annotations;

namespace TourBooking.ApiService.Contracts;

/// <summary>
/// Represents a request to create a new tour.
/// </summary>
/// <param name="Name">The name/title of the tour.</param>
/// <param name="Description">The description of the tour.</param>
/// <param name="Price">The price per person for the tour.</param>
/// <param name="StartDate">The start date of the tour.</param>
/// <param name="EndDate">The end date of the tour.</param>
[PublicAPI]
public sealed record CreateTourRequest(
    string Name,
    string Description,
    decimal Price,
    DateOnly StartDate,
    DateOnly EndDate);
