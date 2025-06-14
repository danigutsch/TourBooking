using System.Reflection.Emit;

namespace TourBooking.MigrationService;

internal sealed class Migrator(TimeProvider clock, ILogger<Migrator> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogMigratorRunning(clock.GetUtcNow());
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}

internal static partial class MigratorLogger
{
    [LoggerMessage(EventId = 0, EventName = "MigratorStart", Level = LogLevel.Information, Message = "Migrator running at: {Time}")]
    public static partial void LogMigratorRunning(this ILogger<Migrator> logger, DateTimeOffset time);
}
