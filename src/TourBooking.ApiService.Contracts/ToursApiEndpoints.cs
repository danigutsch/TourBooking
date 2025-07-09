using JetBrains.Annotations;

namespace TourBooking.ApiService.Contracts;

/// <summary>
/// Defines the routes for the Tours API.
/// </summary>
[PublicAPI]
public static class ToursApiEndpoints
{
    /// <summary>
    /// The base path for the Tours API.
    /// </summary>
    public const string ToursBasePath = "/tours";

    /// <summary>
    /// The base URI for the Tours API.
    /// </summary>
    public static readonly Uri Tours = new(ToursBasePath, UriKind.Relative);

    /// <summary>
    /// The path to retrieve all tours.
    /// </summary>
    public const string GetToursPath = "/tours";

    /// <summary>
    /// Represents the relative URI for retrieve all tours.
    /// </summary>
    public static readonly Uri GetTours = new(GetToursPath, UriKind.Relative);

    /// <summary>
    /// The path to create a new tour.
    /// </summary>
    public const string CreateTourPath = "/tours";
    
    /// <summary>
    /// Represents the relative URI for creating a tour.
    /// </summary>
    public static readonly Uri CreateTour = new(CreateTourPath, UriKind.Relative);
    
    /// <summary>
    /// The path to retrieve a tour by its unique identifier.
    /// </summary>
    public const string GetTourByIdPath = "/tours/{id:guid}";
    
    /// <summary>
    /// Represents the relative URI for retrieving a tour by its unique identifier.
    /// </summary>
    public static Uri GetTourByIdUri(Guid tourId) => new($"{ToursBasePath}/{tourId}", UriKind.Relative);
}