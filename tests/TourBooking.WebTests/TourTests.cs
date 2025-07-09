using Microsoft.EntityFrameworkCore;
using TourBooking.Tests;
using TourBooking.Tests.Shared;
using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;
using TourBooking.Tours.Persistence;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
public sealed class TourTests
{
    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }
    
    [Test]
    public async Task Gets_Tour_By_Id()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        using var scope = Aspire.ApiFixture.Services.CreateScope();
        var toursContext = scope.ServiceProvider.GetRequiredService<ToursDbContext>();
        var tour = await toursContext.Set<Tour>().FirstAsync(cts.Token);

        // Act
        var response = await Aspire.ApiClient.GetTourById(tour.Id, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Name).IsEqualTo(tour.Name);
        await Assert.That(response.Description).IsEqualTo(tour.Description);
        await Assert.That(response.Price).IsEqualTo(tour.Price);
        await Assert.That(response.StartDate).IsEqualTo(tour.StartDate);
        await Assert.That(response.EndDate).IsEqualTo(tour.EndDate);
    }

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
        var (createdTour, url) = await Aspire.ApiClient.CreateTour(request, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(createdTour).IsNotNull();
        await Assert.That(createdTour.Name).IsEqualTo(request.Name);
        await Assert.That(createdTour.Description).IsEqualTo(request.Description);
        await Assert.That(createdTour.Price).IsEqualTo(request.Price);
        await Assert.That(createdTour.StartDate).IsEqualTo(request.StartDate);
        await Assert.That(createdTour.EndDate).IsEqualTo(request.EndDate);

        await Assert.That(url).IsNotDefault();
    }
}
