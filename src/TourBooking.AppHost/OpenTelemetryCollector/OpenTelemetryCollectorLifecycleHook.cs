using Aspire.Hosting.Lifecycle;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace TourBooking.AppHost.OpenTelemetryCollector;

/// <summary>
/// Lifecycle hook for configuring OpenTelemetry Collector endpoint propagation.
/// </summary>
[UsedImplicitly]
public sealed class OpenTelemetryCollectorLifecycleHook(ILogger<OpenTelemetryCollectorLifecycleHook> logger)
    : IDistributedApplicationLifecycleHook
{
    private const string OtelExporterOtlpEndpoint = "OTEL_EXPORTER_OTLP_ENDPOINT";

    /// <inheritdoc />
    public Task AfterEndpointsAllocatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(appModel);

        var collectorResource = appModel.Resources.OfType<OpenTelemetryCollectorResource>().FirstOrDefault();
        if (collectorResource is null)
        {
            logger.NoResourceFound(nameof(OpenTelemetryCollectorResource));
            return Task.CompletedTask;
        }

        var endpoint = collectorResource.GetEndpoint(OpenTelemetryCollectorResource.OtlpGrpcEndpointName);
        if (!endpoint.Exists)
        {
            logger.NoEndpointForCollector(OpenTelemetryCollectorResource.OtlpGrpcEndpointName);
            return Task.CompletedTask;
        }

        foreach (var resource in appModel.Resources)
        {
            resource.Annotations.Add(new EnvironmentCallbackAnnotation(context =>
            {
                if (context.EnvironmentVariables.ContainsKey(OtelExporterOtlpEndpoint))
                {
                    logger.ForwardTelemetryToCollector(resource.Name);
                    context.EnvironmentVariables[OtelExporterOtlpEndpoint] = endpoint;
                }
            }));
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// Logger methods for <see cref="OpenTelemetryCollectorLifecycleHook"/>.
/// </summary>
public static partial class OpenTelemetryCollectorLifecycleHookLogger
{
    /// <summary>
    /// Logs when no resource is found.
    /// </summary>
    [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "No {ResourceName} resource found.", EventName = "NoResourceFound")]
    public static partial void NoResourceFound(this ILogger logger, string resourceName);

    /// <summary>
    /// Logs when no endpoint is found for the collector.
    /// </summary>
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "No {EndpointName} endpoint for the collector.", EventName = "NoEndpointForCollector")]
    public static partial void NoEndpointForCollector(this ILogger logger, string endpointName);

    /// <summary>
    /// Logs when telemetry is forwarded to the collector.
    /// </summary>
    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "Forwarding telemetry for {ResourceName} to the collector.", EventName = "ForwardTelemetryToCollector")]
    public static partial void ForwardTelemetryToCollector(this ILogger logger, string resourceName);
}
