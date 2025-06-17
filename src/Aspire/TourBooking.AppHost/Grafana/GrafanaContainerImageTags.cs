namespace TourBooking.AppHost.Grafana;

/// <summary>
/// Container image tags and registry information for the Grafana resource.
/// </summary>
public static class GrafanaContainerImageTags
{
    /// <summary>
    /// The container registry for the Grafana image.
    /// </summary>
    public const string Registry = "docker.io";

    /// <summary>
    /// The container image name for Grafana.
    /// </summary>
    public const string Image = "grafana/grafana";

    /// <summary>
    /// The default tag for the Grafana image.
    /// </summary>
    public const string Tag = "latest";
}
