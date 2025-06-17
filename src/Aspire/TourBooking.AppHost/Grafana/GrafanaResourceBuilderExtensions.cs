namespace TourBooking.AppHost.Grafana;

internal static class GrafanaResourceBuilderExtensions
{
    public static IResourceBuilder<GrafanaResource> AddGrafana(this IDistributedApplicationBuilder builder, string name, string configPath, string dashboardsPath)
    {
        var resource = new GrafanaResource(name);
        var resourceBuilder = builder.AddResource(resource)
            .WithImage(GrafanaContainerImageTags.Image, GrafanaContainerImageTags.Tag)
            .WithBindMount(configPath, "/etc/grafana", isReadOnly: true)
            .WithBindMount(dashboardsPath, "/var/lib/grafana/dashboards", isReadOnly: true)
            .WithHttpEndpoint(targetPort: 3000, name: GrafanaResource.HttpEndpointName);

        return resourceBuilder;
    }
}
