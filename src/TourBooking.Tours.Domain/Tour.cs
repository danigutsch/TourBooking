namespace TourBooking.Tours.Domain;

/// <summary>
/// Represents a bike tour offering that customers can book.
/// This class is an aggregate root in the Tours bounded context.
/// </summary>
public sealed class Tour
{
    /// <summary>
    /// Initializes a new instance of the bike tour.
    /// </summary>
    /// <param name="name">The name/title of the tour.</param>
    /// <param name="description">The detailed description of the tour.</param>
    /// <param name="price">The price per person for the tour.</param>
    /// <param name="startDate">The date when the tour begins.</param>
    /// <param name="endDate">The date when the tour concludes.</param>
    /// <param name="clock">Time provider for generating consistent timestamps.</param>
    /// <exception cref="ArgumentNullException">Thrown when the clock parameter is null.</exception>
    public Tour(string name, string description, decimal price, DateOnly startDate, DateOnly endDate, TimeProvider clock)
    {
        ArgumentNullException.ThrowIfNull(clock);

        Id = Guid.CreateVersion7(clock.GetUtcNow());
        Name = name;
        Description = description;
        Price = price;
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Gets the unique identifier for the tour.
    /// Generated as a ULID-like identifier using Version 7 UUID for time-based sorting capabilities.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name/title of the tour.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the detailed description of the tour, including route information and highlights.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the price per person for the tour.
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// Gets the date when the tour begins.
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    /// Gets the date when the tour concludes.
    /// </summary>
    public DateOnly EndDate { get; }
}
