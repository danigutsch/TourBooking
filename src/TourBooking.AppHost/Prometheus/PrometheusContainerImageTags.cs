namespace TourBooking.AppHost.Prometheus;

/// <summary>
/// Provides container image tag constants for Prometheus.
/// </summary>
public static class PrometheusContainerImageTags
{
    /// <summary>
    /// The container registry for Prometheus.
    /// </summary>
    public const string Registry = "docker.io";

    /// <summary>
    /// The container image name for Prometheus.
    /// </summary>
    public const string Image = "prom/prometheus";

    /// <summary>
    /// The container image tag for Prometheus.
    /// </summary>
    public const string Tag = "v3.2.1";
}
