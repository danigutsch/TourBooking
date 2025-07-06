namespace TourBooking.AppHost.Grafana;

/// <summary>
/// Represents a Grafana container resource.
/// </summary>
public sealed class GrafanaResource(string name) : ContainerResource(name)
{
    /// <summary>
    /// The name of the HTTP endpoint for Grafana.
    /// </summary>
    internal const string HttpEndpointName = "http";

    private EndpointReference? _httpEndpoint;

    /// <summary>
    /// Gets the HTTP endpoint reference for Grafana.
    /// </summary>
    public EndpointReference HttpEndpoint => _httpEndpoint ??= new EndpointReference(this, HttpEndpointName);
}
