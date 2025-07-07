using System.Net.Http.Json;
using TourBooking.ApiService.Contracts;
using TourBooking.Tests;
using TourBooking.Tests.Shared;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
public sealed class TourTests
{
    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }

    [Test]
    public async Task Gets_All_Tours()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        // Act
        var response = await Aspire.ApiHttpClient.GetAsync(ToursApiEndpoints.GetTours, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task Creates_Tour()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        // Act
        var request = TourDtoFactory.Create();
        var response = await Aspire.ApiHttpClient.PostAsJsonAsync(ToursApiEndpoints.CreateTour, request, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
    }
}
