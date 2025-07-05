using JetBrains.Annotations;

namespace TourBooking.Web.Contracts;

/// <summary>
/// Defines the routes for the Tours website.
/// </summary>
[PublicAPI]
public static class ToursWebEndpoints
{
    /// <summary>
    /// The base path for the Tours website.
    /// </summary>
    public const string ToursBasePath = "/tours";

    /// <summary>
    /// The base URI for the Tours website.
    /// </summary>
    public static readonly Uri Tours = new(ToursBasePath, UriKind.Relative);

    /// <summary>
    /// The path to retrieve all website.
    /// </summary>
    public const string GetToursPath = "/tours";

    /// <summary>
    /// Represents the relative URI for retrieve all tours.
    /// </summary>
    public static readonly Uri GetTours = new(GetToursPath, UriKind.Relative);

    /// <summary>
    /// The path to create a new tour.
    /// </summary>
    public const string CreateTourPath = "/tours/create";

    /// <summary>
    /// Represents the relative URI for creating a tour.
    /// </summary>
    public static readonly Uri CreateTour = new(CreateTourPath, UriKind.Relative);
}
