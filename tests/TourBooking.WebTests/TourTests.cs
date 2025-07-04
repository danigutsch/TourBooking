using System.Net.Http.Json;
using TourBooking.ApiService.Contracts;
using TourBooking.Tests;
using TourBooking.Tests.Shared;
using TourBooking.Web.Contracts;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
public sealed class TourTests
{
    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }

    private HttpClient ApiClient => Aspire.ApiClient;

    [Test]
    public async Task Gets_All_Tours()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        // Act
        var response = await ApiClient.GetAsync(ToursEndpoints.GetTours, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task CreateTourApiReturnsSuccess()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var today = DateTime.UtcNow.ToDateOnly();

        // Act
        var request = new CreateTourDto("Test Tour", "A wonderful tour", 100.0m, today, today.AddDays(5));
        var response = await ApiClient.PostAsJsonAsync(new Uri("/tours", UriKind.Relative), request, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }
}
