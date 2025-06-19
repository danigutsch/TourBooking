namespace TourBooking.AppHost.Prometheus;

/// <summary>
/// Represents a Prometheus container resource.
/// </summary>
public class PrometheusResource(string name) : ContainerResource(name)
{
    /// <summary>
    /// The name of the HTTP endpoint for Prometheus.
    /// </summary>
    public const string HttpEndpointName = "http";

    private EndpointReference? _httpEndpoint;

    /// <summary>
    /// Gets the HTTP endpoint reference for Prometheus.
    /// </summary>
    public EndpointReference HttpEndpoint => _httpEndpoint ??= new(this, HttpEndpointName);
}
