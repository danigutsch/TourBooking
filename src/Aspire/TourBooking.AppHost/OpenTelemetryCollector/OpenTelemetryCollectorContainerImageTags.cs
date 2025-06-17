namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Container image tags and registry information for the OpenTelemetry Collector resource.
/// </summary>
public static class OpenTelemetryCollectorContainerImageTags
{
    /// <summary>
    /// The container registry for the OpenTelemetry Collector image.
    /// </summary>
    public const string Registry = "ghcr.io";

    /// <summary>
    /// The container image name for the OpenTelemetry Collector.
    /// </summary>
    public const string Image = "open-telemetry/opentelemetry-collector-releases/opentelemetry-collector-contrib";

    /// <summary>
    /// The default tag for the OpenTelemetry Collector image.
    /// </summary>
    public const string Tag = "0.128.0";
}
