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
}
