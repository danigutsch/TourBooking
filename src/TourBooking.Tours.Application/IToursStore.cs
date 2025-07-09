using TourBooking.Tours.Domain;

namespace TourBooking.Tours.Application;

/// <summary>
/// Defines a contract for managing and persisting tour entities.
/// </summary>
public interface IToursStore
{
    /// <summary>
    /// Adds a new tour to the store.
    /// </summary>
    /// <param name="tour">The tour to add.</param>
    void Add(Tour tour);

    /// <summary>
    /// Retrieves all tours.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns></returns>
    Task<IReadOnlyList<Tour>> GetAll(CancellationToken ct);

    /// <summary>
    /// Retrieves a tour by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tour.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    Task<Tour> GetById(Guid id, CancellationToken ct);
}
