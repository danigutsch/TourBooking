namespace TourBooking.Tours.Application;

/// <summary>
/// Provides functionality to manage and execute database migration scripts.
/// </summary>
/// <remarks>This interface defines methods for executing SQL migration scripts, allowing for database schema
/// updates or data transformations. Implementations of this interface should ensure proper handling of errors and
/// cancellation requests during script execution.</remarks>
public interface IMigrationManager
{
    /// <summary>
    /// Executes the specified SQL migration script.
    /// </summary>
    /// <param name="sqlScript">The SQL script to be executed.</param>
    /// <param name="ct">The cancellation token for the operation.</param>
    Task ExecuteMigrationScript(string sqlScript, CancellationToken ct);

    /// <summary>
    /// Applies the latest migration script from the specified directory and returns the script name.
    /// </summary>
    /// <param name="scriptsPath">Path to the directory containing migration scripts.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The name of the applied migration script.</returns>
    /// <exception cref="InvalidOperationException">Thrown if directory or script is missing or empty.</exception>
    Task<string> ApplyLatestMigration(string scriptsPath, CancellationToken ct);
}
