namespace TourBooking.AppHost;

/// <summary>
/// Provides a collection of predefined environment variable names used throughout the Aspire application.
/// </summary>
/// <remarks>This class contains constant string values for environment variables that control application behavior,
/// feature flags, and configuration settings. These constants ensure consistency and prevent typos when accessing
/// environment variables across the application.</remarks>
internal static class EnvironmentVariables
{
    /// <summary>
    /// Environment variable that controls whether development tools are included in the application startup.
    /// When set to "true", additional development tools like PgWeb, RedisInsight, and Redis Commander are started.
    /// Should be set to "false" or omitted in production environments for security reasons.
    /// </summary>
    public const string AspireIncludeDevTools = "ASPIRE_INCLUDE_DEV_TOOLS";
}
