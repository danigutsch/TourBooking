using Microsoft.EntityFrameworkCore;
using TourBooking.Tests;
using TourBooking.Tests.Shared;
using TourBooking.Tours.Domain;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
public sealed class TourTests : TourTestBase
{
    [Test]
    public async Task Gets_Tour_By_Id()
    {
        // Arrange
        var tour = await Set<Tour>().FirstAsync(Token);

        // Act
        var response = await Aspire.ApiClient.GetTourById(tour.Id, Token);

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

        // Act
        var response = await ApiClient.GetAllTours(Token);

        // Assert
        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task Creates_Tour()
    {
        // Arrange

        // Act
        var request = TourDtoFactory.Create();
        var (createdTour, url) = await ApiClient.CreateTour(request, Token);

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