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
        var response = await Aspire.ApiClient.GetAllTours(TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task Creates_Tour()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        // Act
        var request = TourDtoFactory.Create();
        var createdTour = await Aspire.ApiClient.CreateTour(request, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(createdTour).IsNotNull();
        await Assert.That(createdTour.Name).IsEqualTo(request.Name);
        await Assert.That(createdTour.Description).IsEqualTo(request.Description);
        await Assert.That(createdTour.Price).IsEqualTo(request.Price);
        await Assert.That(createdTour.StartDate).IsEqualTo(request.StartDate);
        await Assert.That(createdTour.EndDate).IsEqualTo(request.EndDate);
    }
}
