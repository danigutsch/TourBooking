namespace TourBooking.Tours.Application;

/// <summary>
/// Represents a unit of work that encapsulates changes to be persisted to a data store.
/// </summary>
/// <remarks>This interface is typically used to coordinate changes across multiple repositories or entities
/// within a single transaction. Implementations of <see cref="IUnitOfWork"/> ensure that all changes are committed
/// together or rolled back in case of an error.</remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all pending changes to the underlying data store asynchronously.
    /// </summary>
    /// <remarks>This method saves any modifications made to the tracked entities in the current context.
    /// Changes are committed to the data store in a single transaction, if supported by the underlying
    /// provider.</remarks>
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task SaveChanges(CancellationToken ct);
}
