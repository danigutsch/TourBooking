using JetBrains.Annotations;

namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Represents an OpenTelemetry Collector resource for distributed application hosting.
/// </summary>
/// <param name="name">The name of the OpenTelemetry Collector resource.</param>
[PublicAPI]
public sealed class OpenTelemetryCollectorResource(string name) : ContainerResource(name)
{
    internal const string OtlpGrpcEndpointName = "grpc";
    internal const string OtlpHttpEndpointName = "http";
    internal const string HealthCheckEndpointName = "health";
    internal const string ZPagesEndpointName = "zpages";

    private EndpointReference? _otlpGrpcEndpoint;
    private EndpointReference? _otlpHttpEndpoint;
    private EndpointReference? _healthCheckEndpoint;
    private EndpointReference? _zPagesEndpoint;

    /// <summary>
    /// Gets the gRPC endpoint reference for OTLP.
    /// </summary>
    public EndpointReference OtlpGrpcEndpoint => _otlpGrpcEndpoint ??= new EndpointReference(this, OtlpGrpcEndpointName);

    /// <summary>
    /// Gets the HTTP endpoint reference for OTLP.
    /// </summary>
    public EndpointReference OtlpHttpEndpoint => _otlpHttpEndpoint ??= new EndpointReference(this, OtlpHttpEndpointName);

    /// <summary>
    /// Gets the health check endpoint reference.
    /// </summary>
    public EndpointReference HealthCheckEndpoint => _healthCheckEndpoint ??= new EndpointReference(this, HealthCheckEndpointName);

    /// <summary>
    /// Gets the zPages endpoint reference.
    /// </summary>
    public EndpointReference ZPagesEndpoint => _zPagesEndpoint ??= new EndpointReference(this, ZPagesEndpointName);
}
