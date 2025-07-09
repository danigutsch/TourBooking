using System.Net.Http.Json;
using System.Text.Json;
using JetBrains.Annotations;
using TourBooking.ApiService.Contracts.Models;

namespace TourBooking.ApiService.Contracts;

/// <summary>
/// A client for interacting with the Tours API.
/// </summary>
/// <remarks>
/// This client provides methods for performing operations on the Tours API,
/// such as creating a new tour.
/// </remarks>
[UsedImplicitly]
[PublicAPI]
public sealed class ToursApiClient(HttpClient httpClient)
{
    /// <summary>
    /// Creates a new tour.
    /// </summary>
    /// <param name="request">The data transfer object containing the details of the tour to create.</param>
    /// <param name="ct">The cancellation token to cancel the operation.</param>
    /// <returns>A tuple containing the created tour data and the location URI of the created resource.</returns>
    public async Task<(GetTourDto CreatedTour, Uri LocationUri)> CreateTour(CreateTourDto request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync(ToursApiEndpoints.CreateTour, request, ToursApiJsonContext.Default.CreateTourDto, ct).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var locationUri = response.Headers.Location
                          ?? throw new InvalidOperationException("Location header is missing from the create tour response.");

        var createdTour = await response.Content.ReadFromJsonAsync(ToursApiJsonContext.Default.GetTourDto, ct).ConfigureAwait(false)
                          ?? throw new JsonException("Failed to deserialize created tour data.");

        return (createdTour, locationUri);
    }

    /// <summary>
    /// Retrieves all tours.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>An array of tour data transfer objects.</returns>
    public async Task<GetTourDto[]> GetAllTours(CancellationToken ct)
    {
        var response = await httpClient.GetAsync(ToursApiEndpoints.GetTours, ct).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var tours = await response.Content.ReadFromJsonAsync(ToursApiJsonContext.Default.GetTourDtoArray, ct).ConfigureAwait(false)
                    ?? throw new JsonException("Failed to deserialize tours data.");

        return tours;
    }

    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <param name="tourId">The unique identifier of the tour.</param>
    public async Task<GetTourDto> GetTourById(Guid tourId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync(ToursApiEndpoints.GetTourByIdUri(tourId), ct).ConfigureAwait(false);
        
        response.EnsureSuccessStatusCode();
        
        var tour = await response.Content.ReadFromJsonAsync(ToursApiJsonContext.Default.GetTourDto, ct).ConfigureAwait(false)
                    ?? throw new JsonException("Failed to deserialize tour data.");
        
        return tour;
    }
}
