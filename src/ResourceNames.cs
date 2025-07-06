namespace TourBooking.Constants;

/// <summary>
/// Provides a collection of predefined resource identifiers for various system components.
/// </summary>
/// <remarks>This class contains constant string values that represent unique identifiers for specific resources
/// such as databases, services, and caching systems. These identifiers can be used to reference or configure the
/// corresponding resources in the application.</remarks>
internal static class ResourceNames
{
    /// <summary>
    /// Represents the identifier for the Redis caching system.
    /// </summary>
    public const string Redis = "redis";
    /// <summary>
    /// Represents the identifier for the RedisInsight tool.
    /// </summary>
    public const string RedisInsight = "redisinsight";
    /// <summary>
    /// Represents the identifier for the Redis Commander tool.
    /// </summary>
    public const string RedisCommander = "rediscommander";
    /// <summary>
    /// Represents the identifier for the PostgreSQL database.
    /// </summary>
    public const string PostgreSql = "postgresql";
    /// <summary>
    /// Represents the identifier for the Redis database.
    /// </summary>
    public const string PgWeb = "pgweb";
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
    /// <summary>
    /// Represents the identifier for the OpenTelemetry Collector service.
    /// </summary>
    public const string OpenTelemetryCollector = "otelcollector";
    /// <summary>
    /// Represents the identifier for the Prometheus monitoring service.
    /// </summary>
    public const string Prometheus = "prometheus";
    /// <summary>
    /// Represents the identifier for the Grafana dashboard service.
    /// </summary>
    public const string Grafana = "grafana";
    /// <summary>
    /// Represents the identifier for the Jaeger tracing service.
    /// </summary>
    public const string Jaeger = "jaeger";
}
