using Microsoft.EntityFrameworkCore;
using System.Data;
using TourBooking.Tours.Application;

namespace TourBooking.Tours.Persistence;

internal sealed class MigrationManager(ToursDbContext dbContext) : IMigrationManager
{
    public async Task ExecuteMigrationScript(string sqlScript, CancellationToken ct)
    {
        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct).ConfigureAwait(false);
        }

#pragma warning disable CA2007
        await using var cmd = connection.CreateCommand();
#pragma warning restore CA2007
#pragma warning disable CA2100
        cmd.CommandText = sqlScript;
#pragma warning restore CA2100
        cmd.CommandType = CommandType.Text;
        await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Applies the latest migration script from the specified directory and returns the script name.
    /// </summary>
    /// <param name="scriptsPath">Path to the directory containing migration scripts.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The name of the applied migration script.</returns>
    /// <exception cref="InvalidOperationException">Thrown if directory or script is missing or empty.</exception>
    public async Task<string> ApplyLatestMigration(string scriptsPath, CancellationToken ct)
    {
        if (!Directory.Exists(scriptsPath))
        {
            throw new InvalidOperationException($"Migration scripts directory does not exist: {scriptsPath}");
        }

        var files = new DirectoryInfo(scriptsPath).GetFiles("*.sql", SearchOption.TopDirectoryOnly);
        if (files.Length == 0)
        {
            throw new InvalidOperationException($"No migration script found in the specified directory: {scriptsPath}");
        }

        var lastMigration = files.OrderByDescending(file => file.Name).First();
        var migrationScript = await File.ReadAllTextAsync(lastMigration.FullName, ct).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(migrationScript))
        {
            throw new InvalidOperationException($"Migration script is empty: {lastMigration.FullName}");
        }

        await ExecuteMigrationScript(migrationScript, ct).ConfigureAwait(false);
        return lastMigration.Name;
    }
}
