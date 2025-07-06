using System.Diagnostics;
using JetBrains.Annotations;
using TourBooking.Tours.Application;

namespace TourBooking.MigrationService;

[MustDisposeResource]
internal sealed class Migrator(
    IServiceProvider serviceProvider,
    IHostEnvironment hostEnvironment,
    IHostApplicationLifetime hostApplicationLifetime,
    IConfiguration configuration,
    ILogger<Migrator> logger
    )
    : BackgroundService
{
    /// <summary>
    /// Path to the directory containing the migration scripts for the Tours database.
    /// THIS IS REQUIRED.
    /// </summary>
    private const string ToursMigrationsPath = "TOURS_MIGRATIONS_PATH";

    public const string ActivitySourceName = "TourBooking.MigrationService";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity(hostEnvironment.ApplicationName, ActivityKind.Client);

        logger.MigratorRunning();

        var scriptsPath = configuration.GetValue<string>(ToursMigrationsPath)
                          ?? throw new InvalidOperationException("Tours database migration scripts path is not configured.");

        scriptsPath = scriptsPath.Replace('/', Path.DirectorySeparatorChar)
                                 .Replace('\\', Path.DirectorySeparatorChar);

        scriptsPath = Path.Combine(AppContext.BaseDirectory, scriptsPath);

        using var scope = serviceProvider.CreateScope();
        var migrationManager = scope.ServiceProvider.GetRequiredService<IMigrationManager>();

        string? scriptName = null;
        try
        {
            scriptName = await migrationManager.ApplyLatestMigration(scriptsPath, stoppingToken);
            logger.MigrationApplied(scriptName);
        }
        catch (InvalidOperationException ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            if (ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
            {
                logger.DirectoryDoesNotExist(scriptsPath);
            }
            else if (ex.Message.Contains("No migration script found", StringComparison.OrdinalIgnoreCase))
            {
                logger.NoScriptFound(scriptsPath);
            }
            else if (ex.Message.Contains("Migration script is empty", StringComparison.OrdinalIgnoreCase))
            {
                logger.NoScriptFound(scriptName ?? scriptsPath);
            }
            else
            {
                logger.NoScriptFound(scriptsPath);
            }
            throw;
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
        finally
        {
            logger.MigratorStopping();
        }

        hostApplicationLifetime.StopApplication();
    }

    public override void Dispose()
    {
        _activitySource.Dispose();
        base.Dispose();
    }
}

internal static partial class MigratorLogger
{
    [LoggerMessage(EventId = 0, EventName = "MigratorStart", Level = LogLevel.Information, Message = "Migrator started")]
    public static partial void MigratorRunning(this ILogger<Migrator> logger);

    [LoggerMessage(EventId = 2, EventName = "MigrationsDirectoryDoesNotExist", Level = LogLevel.Warning, Message = "The directory '{Path}' does not exist.")]
    public static partial void DirectoryDoesNotExist(this ILogger<Migrator> logger, string path);

    [LoggerMessage(EventId = 3, EventName = "NoMigrationScriptFound", Level = LogLevel.Warning, Message = "No migration script found in '{Path}'.")]
    public static partial void NoScriptFound(this ILogger<Migrator> logger, string path);

    [LoggerMessage(EventId = 4, EventName = "ApplyingMigration", Level = LogLevel.Information, Message = "Applying migration script '{ScriptName}'")]
    public static partial void ApplyingMigration(this ILogger<Migrator> logger, string scriptName);

    [LoggerMessage(EventId = 5, EventName = "MigrationApplied", Level = LogLevel.Information, Message = "Migration script '{ScriptName}' applied successfully")]
    public static partial void MigrationApplied(this ILogger<Migrator> logger, string scriptName);

    [LoggerMessage(EventId = 6, EventName = "MigratorStopping", Level = LogLevel.Information, Message = "Migrator is stopping")]
    public static partial void MigratorStopping(this ILogger<Migrator> logger);
}
