using JetBrains.Annotations;
using TourBooking.ApiService.Contracts;

namespace TourBooking.Web;

/// <summary>
/// A client for interacting with the Tours API.
/// </summary>
/// <remarks>
/// This client provides methods for performing operations on the Tours API,
/// such as creating a new tour.
/// </remarks>
[UsedImplicitly]
internal sealed class ToursApiClient(HttpClient httpClient)
{
    public async Task CreateTour(CreateTourRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("/tours", request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
