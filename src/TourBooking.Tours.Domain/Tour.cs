﻿namespace TourBooking.Tours.Domain;

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
    /// <exception cref="ArgumentNullException">Thrown when the clock parameter is null.</exception>
    public Tour(string name, string description, decimal price, DateOnly startDate, DateOnly endDate)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length is < 3 or > 100)
        {
            throw new ArgumentException("Name length must be between 3 and 100 characters and cannot be null, empty, or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null, empty, or consist only of whitespace.", nameof(description));
        }

        if (description.Length is < 10 or > 500)
        {
            throw new ArgumentException("Description length must be between 10 and 500 characters.", nameof(description));
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.", nameof(price));
        }

        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be after start date.", nameof(endDate));
        }

        Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow());
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
    /// Gets the detailed description of the tour.
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
