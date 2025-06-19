namespace TourBooking.AppHost.Jaeger;

/// <summary>
/// Container image tags for the Jaeger resource.
/// </summary>
public static class JaegerContainerImageTags
{
    /// <summary>
    /// The container registry for Jaeger.
    /// </summary>
    public const string Registry = "docker.io";

    /// <summary>
    /// The container image for Jaeger.
    /// </summary>
    public const string Image = "jaegertracing/jaeger";

    /// <summary>
    /// The container tag for Jaeger.
    /// </summary>
    public const string Tag = "2.7.0";
}
