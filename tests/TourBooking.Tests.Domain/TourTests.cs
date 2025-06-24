using TourBooking.Tours.Domain;

namespace TourBooking.Tests.Domain;

public class TourTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(99, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
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

    [Theory]
    [InlineData(0, false)]
    [InlineData(9, false)]
    [InlineData(10, true)]
    [InlineData(11, true)]
    [InlineData(499, true)]
    [InlineData(500, true)]
    [InlineData(501, false)]
    public void Description_Length_Has_To_Be_Between_10_And_500_Characters(int length, bool canCreate)
    {
        // Arrange
        var description = new string('d', length);
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = Record.Exception(() => new Tour("Valid Name", description, 100.0m, today, endDate));

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

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(0.01, true)]
    [InlineData(1, true)]
    [InlineData(100, true)]
    public void Price_Must_Be_Greater_Than_Zero(decimal price, bool canCreate)
    {
        // Arrange
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = Record.Exception(() => new Tour("Valid Name", "A valid description", price, today, endDate));

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

    [Theory]
    [InlineData("2025-06-24", "2025-06-23", false)] // end before start
    [InlineData("2025-06-24", "2025-06-24", false)]  // same day (not allowed)
    [InlineData("2025-06-24", "2025-06-25", true)]  // end after start
    public void EndDate_Must_Be_After_StartDate(string start, string end, bool canCreate)
    {
        // Arrange
        var startDate = DateOnly.Parse(start, System.Globalization.CultureInfo.InvariantCulture);
        var endDate = DateOnly.Parse(end, System.Globalization.CultureInfo.InvariantCulture);

        // Act
        var action = Record.Exception(() => new Tour("Valid Name", "A valid description", 100.0m, startDate, endDate));

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
