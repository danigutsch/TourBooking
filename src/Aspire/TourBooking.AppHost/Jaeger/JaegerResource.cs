namespace TourBooking.AppHost.Jaeger;

/// <summary>
/// Represents a Jaeger all-in-one container resource.
/// </summary>
public sealed class JaegerResource(string name) : ContainerResource(name)
{
    /// <summary>
    /// The name of the Jaeger UI endpoint.
    /// </summary>
    internal const string UiEndpointName = "ui";

    /// <summary>
    /// The name of the OTLP gRPC endpoint.
    /// </summary>
    internal const string OtlpGrpcEndpointName = "otlp-grpc";

    /// <summary>
    /// The name of the OTLP HTTP endpoint.
    /// </summary>
    internal const string OtlpHttpEndpointName = "otlp-http";

    /// <summary>
    /// The name of the sampling endpoint.
    /// </summary>
    internal const string SamplingEndpointName = "sampling";

    private EndpointReference? _uiEndpoint;
    private EndpointReference? _otlpGrpcEndpoint;
    private EndpointReference? _otlpHttpEndpoint;
    private EndpointReference? _samplingEndpoint;

    /// <summary>
    /// Gets the Jaeger UI endpoint reference.
    /// </summary>
    public EndpointReference UiEndpoint => _uiEndpoint ??= new(this, UiEndpointName);

    /// <summary>
    /// Gets the OTLP gRPC endpoint reference.
    /// </summary>
    public EndpointReference OtlpGrpcEndpoint => _otlpGrpcEndpoint ??= new(this, OtlpGrpcEndpointName);

    /// <summary>
    /// Gets the OTLP HTTP endpoint reference.
    /// </summary>
    public EndpointReference OtlpHttpEndpoint => _otlpHttpEndpoint ??= new(this, OtlpHttpEndpointName);

    /// <summary>
    /// Gets the sampling endpoint reference.
    /// </summary>
    public EndpointReference SamplingEndpoint => _samplingEndpoint ??= new(this, SamplingEndpointName);
}
