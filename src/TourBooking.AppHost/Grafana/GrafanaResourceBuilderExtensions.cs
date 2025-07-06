namespace TourBooking.AppHost.Grafana;

internal static class GrafanaResourceBuilderExtensions
{
    /// <summary>
    /// Adds a Grafana resource to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder to add the Grafana resource to.</param>
    /// <param name="name">The name of the Grafana resource.</param>
    /// <param name="configPath">The local path to the Grafana configuration directory to bind mount.</param>
    /// <param name="dashboardsPath">The local path to the directory containing Grafana dashboards to bind mount.</param>
    /// <returns>An <see cref="IResourceBuilder{GrafanaResource}"/> for further configuration of the Grafana resource.</returns>
    public static IResourceBuilder<GrafanaResource> AddGrafana(this IDistributedApplicationBuilder builder, string name, string configPath, string dashboardsPath)
    {
        var resource = new GrafanaResource(name);
        var resourceBuilder = builder.AddResource(resource)
            .WithImage(GrafanaContainerImageTags.Image, GrafanaContainerImageTags.Tag)
            .WithImageRegistry(GrafanaContainerImageTags.Registry)
            .WithBindMount(configPath, "/etc/grafana", isReadOnly: true)
            .WithBindMount(dashboardsPath, "/var/lib/grafana/dashboards", isReadOnly: true)
            .WithEnvironment("DASHBOARD_OTLP_API_KEY", "AppHost__OtlpApiKey")
            .WithHttpEndpoint(targetPort: 3000, name: GrafanaResource.HttpEndpointName)
            .WithHttpHealthCheck("/api/health");

        return resourceBuilder;
    }
}
