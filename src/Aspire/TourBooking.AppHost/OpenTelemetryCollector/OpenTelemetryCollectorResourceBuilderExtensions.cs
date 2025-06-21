using Microsoft.Extensions.Hosting;

namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Provides extension methods for adding OpenTelemetry Collector resources to the application model.
/// </summary>
public static class OpenTelemetryCollectorResourceBuilderExtensions
{
    private const string DashboardOtlpUrlVariableName = "DASHBOARD_OTLP_ENDPOINT_URL";
    private const string DashboardOtlpApiKeyVariableName = "AppHost:OtlpApiKey";
    private const string DashboardOtlpUrlDefaultValue = "http://localhost:18889";

    /// <summary>
    /// Adds an OpenTelemetry Collector resource to the application model.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="configFileLocation">The path to the collector configuration file.</param>
    /// <returns>The resource builder for the OpenTelemetry Collector resource.</returns>
    public static IResourceBuilder<OpenTelemetryCollectorResource> AddOpenTelemetryCollector(this IDistributedApplicationBuilder builder, string name, string configFileLocation)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddOpenTelemetryCollectorInfrastructure();

        var url = builder.Configuration[DashboardOtlpUrlVariableName] ?? DashboardOtlpUrlDefaultValue;
        var isHttpsEnabled = url.StartsWith("https", StringComparison.OrdinalIgnoreCase);

        var dashboardOtlpEndpoint = new HostUrl(url);

        var resource = new OpenTelemetryCollectorResource(name);
        var resourceBuilder = builder.AddResource(resource)
            .WithImage(OpenTelemetryCollectorContainerImageTags.Image, OpenTelemetryCollectorContainerImageTags.Tag)
            .WithImageRegistry(OpenTelemetryCollectorContainerImageTags.Registry)
            .WithHttpEndpoint(
                targetPort: 4317,
                name: OpenTelemetryCollectorResource.OtlpGrpcEndpointName)
            .WithHttpEndpoint(
                targetPort: 4318,
                name: OpenTelemetryCollectorResource.OtlpHttpEndpointName)
            .WithHttpEndpoint(
                targetPort: 13133,
                name: OpenTelemetryCollectorResource.HealthCheckEndpointName)
            .WithHttpEndpoint(
                targetPort: 55679,
                name: OpenTelemetryCollectorResource.ZPagesEndpointName)
            .WithEndpoint(
                targetPort: 1777,
                name: OpenTelemetryCollectorResource.PprofEndpointName)
            .WithBindMount(configFileLocation, "/etc/otelcol-contrib/config.yaml")
            .WithEnvironment("ASPIRE_ENDPOINT", $"{dashboardOtlpEndpoint}")
            .WithEnvironment("ASPIRE_API_KEY", builder.Configuration[DashboardOtlpApiKeyVariableName])
            .WithEnvironment("ASPIRE_INSECURE", isHttpsEnabled ? "false" : "true");

        if (isHttpsEnabled && builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
        {
            resourceBuilder.RunWithHttpsDevCertificate("HTTPS_CERT_FILE", "HTTPS_CERT_KEY_FILE", (_, _) =>
            {
                // Set TLS details using YAML path via the command line. This allows the values to be added to the existing config file.
                // Setting the values in the config file doesn't work because adding the "tls" section always enables TLS, even if there is no cert provided.
                resourceBuilder.WithArgs(
                    @"--config=yaml:receivers::otlp::protocols::grpc::tls::cert_file: ""dev-certs/dev-cert.pem""",
                    @"--config=yaml:receivers::otlp::protocols::grpc::tls::key_file: ""dev-certs/dev-cert.key""",
                    "--config=/etc/otelcol-contrib/config.yaml");
            });
        }

        return resourceBuilder;
    }
}
