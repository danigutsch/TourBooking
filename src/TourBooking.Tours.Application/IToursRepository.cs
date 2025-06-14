using TourBooking.Tours.Domain;

namespace TourBooking.Tours.Application;

/// <summary>
/// Defines a contract for managing and persisting tour entities.
/// </summary>
public interface IToursRepository
{
    /// <summary>
    /// Adds a new tour to the repository.
    /// </summary>
    /// <param name="tour">The tour to add.</param>
    void Add(Tour tour);
}
