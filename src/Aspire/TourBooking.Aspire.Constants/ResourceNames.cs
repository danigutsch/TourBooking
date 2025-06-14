namespace TourBooking.Aspire.Constants;

/// <summary>
/// Provides a collection of predefined resource identifiers for various system components.
/// </summary>
/// <remarks>This class contains constant string values that represent unique identifiers for specific resources
/// such as databases, services, and caching systems. These identifiers can be used to reference or configure the
/// corresponding resources in the application.</remarks>
public static class ResourceNames
{
    /// <summary>
    /// Represents the identifier for the Redis caching system.
    /// </summary>
    public const string Redis = "redis";
    /// <summary>
    /// Represents the identifier for the PostgreSQL database.
    /// </summary>
    public const string PostgreSql = "postgresql";
    /// <summary>
    /// Represents the identifier for the Tours database.
    /// </summary>
    public const string ToursDatabase = "tourbooking";
    /// <summary>
    /// Represents the identifier for the Api Service.
    /// </summary>
    public const string ApiService = "apiservice";
    /// <summary>
    /// Represents the identifier for the Web Frontend.
    /// </summary>
    public const string WebFrontend = "webfrontend";
    /// <summary>
    /// Represents the identifier for the Migration Service.
    /// </summary>
    public const string MigrationService = "migrationservice";
}
