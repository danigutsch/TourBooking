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
            .WithHttpEndpoint(targetPort: 16686, name: JaegerResource.UiEndpointName)
            .WithHttpEndpoint(targetPort: 4317, name: JaegerResource.OtlpGrpcEndpointName)
            .WithHttpEndpoint(targetPort: 4318, name: JaegerResource.OtlpHttpEndpointName)
            .WithHttpEndpoint(targetPort: 5778, name: JaegerResource.SamplingEndpointName)
            .WithImage(JaegerContainerImageTags.Image, JaegerContainerImageTags.Tag)
            .WithImageRegistry(JaegerContainerImageTags.Registry)
            .WithHttpHealthCheck(path: "/", endpointName: JaegerResource.UiEndpointName);
        return resourceBuilder;
    }
}
