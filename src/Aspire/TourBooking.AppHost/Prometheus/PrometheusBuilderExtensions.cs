namespace TourBooking.AppHost.Prometheus;

/// <summary>
/// Provides extension methods for adding Prometheus to the distributed application.
/// </summary>
public static class PrometheusBuilderExtensions
{
    /// <summary>
    /// Adds a Prometheus resource to the distributed application.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The resource name.</param>
    /// <param name="configPath">The path to the Prometheus config directory.</param>
    /// <returns>The resource builder for Prometheus.</returns>
    public static IResourceBuilder<PrometheusResource> AddPrometheus(this IDistributedApplicationBuilder builder, string name, string configPath)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var prometheusResource = new PrometheusResource(name);
        var resourceBuilder = builder.AddResource(prometheusResource)
            .WithImage(PrometheusContainerImageTags.Image, PrometheusContainerImageTags.Tag)
            .WithImageRegistry(PrometheusContainerImageTags.Registry)
            .WithBindMount(configPath, "/etc/prometheus", isReadOnly: true)
            .WithArgs("--web.enable-otlp-receiver", "--config.file=/etc/prometheus/prometheus.yml")
            .WithHttpEndpoint(targetPort: 9090, name: PrometheusResource.HttpEndpointName)
            .WithHttpHealthCheck("/-/healthy");
        return resourceBuilder;
    }
}
