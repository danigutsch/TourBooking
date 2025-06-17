namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Represents an OpenTelemetry Collector resource for distributed application hosting.
/// </summary>
/// <param name="name">The name of the OpenTelemetry Collector resource.</param>
public sealed class OpenTelemetryCollectorResource(string name) : ContainerResource(name)
{
    /// <summary>
    /// The gRPC endpoint name for OTLP.
    /// </summary>
    public const string OtlpGrpcEndpointName = "grpc";

    /// <summary>
    /// The HTTP endpoint name for OTLP.
    /// </summary>
    public const string OtlpHttpEndpointName = "http";
}
