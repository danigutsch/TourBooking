using Aspire.Hosting.Lifecycle;

namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Provides extension methods for adding OpenTelemetry Collector infrastructure services.
/// </summary>
public static class OpenTelemetryCollectorServiceExtensions
{
    /// <summary>
    /// Adds the OpenTelemetry Collector infrastructure lifecycle hook to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <returns>The distributed application builder.</returns>
    public static IDistributedApplicationBuilder AddOpenTelemetryCollectorInfrastructure(this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddLifecycleHook<OpenTelemetryCollectorLifecycleHook>();

        return builder;
    }
}
