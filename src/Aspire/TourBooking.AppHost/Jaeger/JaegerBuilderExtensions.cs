using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace TourBooking.AppHost.Jaeger;

/// <summary>
/// Extension methods for adding a Jaeger resource to the distributed application.
/// </summary>
public static class JaegerBuilderExtensions
{
    /// <summary>
    /// Adds a Jaeger all-in-one container resource to the distributed application.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The resource name.</param>
    /// <returns>The resource builder for Jaeger.</returns>
    public static IResourceBuilder<JaegerResource> AddJaeger(this IDistributedApplicationBuilder builder, [ResourceName] string name)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var resource = new JaegerResource(name);
        var resourceBuilder = builder.AddResource(resource)
            .WithEndpoint(targetPort: 16686, name: JaegerResource.UiEndpointName, scheme: "http")
            .WithEndpoint(targetPort: 4317, name: JaegerResource.OtlpGrpcEndpointName, scheme: "http")
            .WithEndpoint(targetPort: 4318, name: JaegerResource.OtlpHttpEndpointName, scheme: "http")
            .WithEndpoint(targetPort: 5778, name: JaegerResource.SamplingEndpointName, scheme: "http")
            .WithImage(JaegerContainerImageTags.Image, JaegerContainerImageTags.Tag)
            .WithImageRegistry(JaegerContainerImageTags.Registry)
            .WithHttpHealthCheck(path: "/", endpointName: JaegerResource.UiEndpointName);
        return resourceBuilder;
    }
}
