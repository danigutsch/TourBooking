using TourBooking.Tours.Domain;

namespace TourBooking.Tests.Domain;

public class TourTests
{
    [Fact]
    public void Can_Create_Tour()
    {
        var tour = new Tour("Test Tour", "This is a test tour", 100.0m, DateOnly.MinValue, DateOnly.MaxValue);

        Assert.NotNull(tour);
    }
}
