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

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(99, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    [InlineData(102, false)]
    public void Name_Length_Has_To_Be_Between_3_And_100_Characters(int length, bool canCreate)
    {
        // Arrange
        var name = new string('a', length);
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = Record.Exception(() => new Tour(name, "A valid description", 100.0m, today, endDate));

        // Assert
        if (canCreate)
        {
            Assert.Null(action);
        }
        else
        {
            Assert.IsType<ArgumentException>(action);
        }
    }
}
