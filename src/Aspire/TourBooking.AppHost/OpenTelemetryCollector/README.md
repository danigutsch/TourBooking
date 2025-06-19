# OpenTelemetry Collector Integration

Adds a vendor-neutral telemetry pipeline to the TourBooking Aspire solution, enabling collection, processing, and export of traces, metrics, and logs to multiple backends.

## Getting started

### Prerequisites
- OpenTelemetry Collector container or binary
- At least one backend (e.g., Jaeger, Prometheus, Seq, etc.)

### How to use in TourBooking
- The collector is configured in `otelcollector/config.yaml`.
- The AppHost registers the collector as a resource and sets up dependencies on other telemetry backends.

## Usage example

In the _AppHost.cs_ file, add the collector and reference it from other resources:

```csharp
var otelCollector = builder
    .AddOpenTelemetryCollector(ResourceNames.OpenTelemetryCollector, "../../../otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp")
    .WaitFor(prometheus)
    .WaitFor(grafana)
    .WaitFor(jaeger);
```

## Configuration

The main configuration file is `otelcollector/config.yaml`. It consists of:
- **receivers**: How telemetry is received (e.g., OTLP gRPC/HTTP)
- **processors**: Optional steps (batching, filtering, etc.)
- **exporters**: Where telemetry is sent (e.g., Jaeger, Prometheus, Seq)
- **extensions**: Extra features (health checks, zPages, pprof)
- **service**: Pipelines and enabled extensions

### Example config
```yaml
receivers:
  otlp:
    protocols:
      grpc:
        endpoint: 0.0.0.0:4317
      http:
        endpoint: 0.0.0.0:4318
processors:
  batch:
exporters:
  jaeger:
    endpoint: jaeger:4317
    tls:
      insecure: true
  otlphttp/prometheus:
    endpoint: ${env:PROMETHEUS_ENDPOINT}
    tls:
      insecure: true
extensions:
  health_check:
    endpoint: 0.0.0.0:13133
  zpages:
    endpoint: 0.0.0.0:55679
  pprof:
    endpoint: 0.0.0.0:1777
service:
  extensions: [health_check, zpages, pprof]
  pipelines:
    traces:
      receivers: [otlp]
      processors: [batch]
      exporters: [jaeger]
    metrics:
      receivers: [otlp]
      processors: [batch]
      exporters: [otlphttp/prometheus]
```

### Health checks
- The collector exposes a health check endpoint (default: `:13133`).
- Each exporter/receiver may have its own health or metrics endpoint.

### Adding exporters
To send telemetry to a new backend, add an exporter under `exporters:` and include it in the appropriate pipeline in `service:`.

### Limitations
- Not all backends support all telemetry types (e.g., Jaeger supports only traces).
- Logs can be exported to sinks like Seq, Loki, or Elasticsearch.

## Additional documentation
- [OpenTelemetry Collector Documentation](https://opentelemetry.io/docs/collector/)
- [Aspire Telemetry Architecture](https://github.com/dotnet/aspire/blob/main/docs/open-telemetry-architecture.md)

## Feedback & contributing

For issues or suggestions, please open an issue in this repository.
