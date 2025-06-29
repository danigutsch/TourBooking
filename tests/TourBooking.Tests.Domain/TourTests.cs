using TourBooking.Tours.Domain;
using TUnit.Assertions.AssertConditions.Throws;
using static TourBooking.Tests.TestCategories;

namespace TourBooking.Tests.Domain;

[Category(Unit)]
public class TourTests
{
    private const int MinNameLength = 3;
    private const int MaxNameLength = 100;
    private const int MinDescriptionLength = 10;
    private const int MaxDescriptionLength = 500;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(101)]
    [Arguments(102)]
    public async Task Name_Length_Has_To_Be_Between_3_And_100_Characters(int length)
    {
        // Arrange
        var name = new string('a', length);
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = () => new Tour(name, "A valid description", 100.0m, today, endDate);

        // Assert
        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments(0)]
    [Arguments(9)]
    [Arguments(501)]
    [Arguments(502)]
    public async Task Description_Length_Has_To_Be_Between_10_And_500_Characters(int length)
    {
        // Arrange
        var description = new string('d', length);
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = () => new Tour("Valid Name", description, 100.0m, today, endDate);

        // Assert
        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    [Arguments(-0.01)]
    public async Task Price_Must_Be_Greater_Than_Zero(decimal price)
    {
        // Arrange
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = () => new Tour("Valid Name", "A valid description", price, today, endDate);

        // Assert
        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments("2025-06-24", "2025-06-23")]
    [Arguments("2025-06-24", "2025-06-24")]
    public async Task EndDate_Must_Be_After_StartDate(string start, string end)
    {
        // Arrange
        var startDate = DateOnly.Parse(start, System.Globalization.CultureInfo.InvariantCulture);
        var endDate = DateOnly.Parse(end, System.Globalization.CultureInfo.InvariantCulture);

        // Act
        var action = () => new Tour("Valid Name", "A valid description", 100.0m, startDate, endDate);

        // Assert
        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task Name_Cannot_Be_Null_Or_Whitespace(string? name)
    {
        // Arrange
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var action = () => new Tour(name!, "A valid description", 100.0m, today, endDate);

        // Assert
        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task Id_Is_Not_Empty_After_Creation()
    {
        // Arrange
        var today = DateTime.UtcNow.ToDateOnly();
        var endDate = today.AddDays(5);

        // Act
        var tour = new Tour("Valid Name", "A valid description", 100.0m, today, endDate);

        // Assert
        await Assert.That(tour.Id).IsNotEqualTo(Guid.Empty);
    }

    [Test]
    [MatrixDataSource]
    public async Task Respects_Invariants(
        [Matrix(3, 4, 5, 98, 99, 100)] int nameLength,
        [Matrix(10, 11, 12, 498, 499, 500)] int descriptionLength,
        [Matrix(0.01, 0.1, 1.0, 100.0)] decimal price,
        [Matrix("2025-06-24", "2025-06-25", "2025-06-26")] string startDateStr,
        [Matrix(1, 2, 10)] int tourDurationInDays)
    {
        // Arrange
        var name = new string('a', nameLength);
        var description = new string('d', descriptionLength);
        var startDate = DateOnly.Parse(startDateStr, System.Globalization.CultureInfo.InvariantCulture);
        var endDate = startDate.AddDays(tourDurationInDays);

        // Act
        var tour = new Tour(name, description, price, startDate, endDate);

        // Assert
        await Assert.That(tour.Name.Length).IsBetween(MinNameLength - 1, MaxNameLength + 1).And.IsEqualTo(nameLength);
        await Assert.That(tour.Description.Length).IsBetween(MinDescriptionLength - 1, MaxDescriptionLength + 1).And.IsEqualTo(descriptionLength);
        await Assert.That(tour.Price).IsGreaterThan(0).And.IsEqualTo(price);
        await Assert.That(tour.StartDate).IsLessThan(endDate).IsEqualTo(startDate);
        await Assert.That(tour.EndDate).IsGreaterThan(startDate).And.IsEqualTo(endDate);
    }
}