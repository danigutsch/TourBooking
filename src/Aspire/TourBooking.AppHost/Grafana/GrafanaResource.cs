namespace TourBooking.AppHost.Grafana;

internal sealed class GrafanaResource(string name) : ContainerResource(name)
{
    internal const string HttpEndpointName = "http";
}
