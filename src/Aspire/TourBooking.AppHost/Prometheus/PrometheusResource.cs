namespace TourBooking.AppHost.Prometheus;

/// <summary>
/// Represents a Prometheus container resource.
/// </summary>
public sealed class PrometheusResource(string name) : ContainerResource(name)
{
    /// <summary>
    /// The name of the HTTP endpoint for Prometheus.
    /// </summary>
    internal const string HttpEndpointName = "http";

    private EndpointReference? _httpEndpoint;

    /// <summary>
    /// Gets the HTTP endpoint reference for Prometheus.
    /// </summary>
    public EndpointReference HttpEndpoint => _httpEndpoint ??= new EndpointReference(this, HttpEndpointName);
}
