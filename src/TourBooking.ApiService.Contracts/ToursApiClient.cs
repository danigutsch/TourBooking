using System.Net.Http.Json;
using System.Text.Json;
using JetBrains.Annotations;

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
    public async Task CreateTour(CreateTourDto request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(ToursApiEndpoints.CreateTour, request, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Retrieves all tours.
    /// </summary>
    public async Task<GetTourDto[]> GetAllTours(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(ToursApiEndpoints.GetTours, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var tours = await response.Content.ReadFromJsonAsync<GetTourDto[]>(cancellationToken).ConfigureAwait(false)
            ?? throw new JsonException("Failed to deserialize tours data.");

        return tours;
    }
}
