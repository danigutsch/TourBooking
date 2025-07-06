# TourBooking.AppHost

The **TourBooking.AppHost** project is the .NET Aspire orchestrator that manages the distributed application infrastructure, service dependencies, and development tooling for the Bike Tours Booking Platform.

## Overview

This project serves as the entry point for running the entire distributed application locally and in development environments. It leverages .NET Aspire to orchestrate containerized services, manage dependencies, and provide comprehensive observability and development tooling.

## Key Responsibilities

- **Service Orchestration**: Manages startup order and dependencies between services
- **Infrastructure Setup**: Configures databases (PostgreSQL), caching (Redis), and observability tools
- **Development Experience**: Provides development tools and database management interfaces
- **Resource Management**: Defines and manages all application resources and their configurations
- **Observability**: Sets up monitoring, logging, and tracing infrastructure

## Architecture Components

### Core Services
The AppHost orchestrates the following application services:
- API Service, Web Frontend, and Migration Service

### Infrastructure Services
- **PostgreSQL**: Primary database
- **Redis**: Distributed caching and session storage
- **OpenTelemetry Collector**: Telemetry data collection and forwarding
- **Prometheus**: Metrics collection and storage
- **Grafana**: Dashboards and visualization
- **Jaeger**: Distributed tracing

### Development Tools (Optional)
- **PgWeb**: PostgreSQL database management interface
- **RedisInsight**: Redis data visualization and management
- **Redis Commander**: Alternative Redis management interface

## Environment Variables

### ASPIRE_INCLUDE_DEV_TOOLS

Controls whether development and management tools are included in the application startup.

- **Type**: `string`
- **Values**: `"true"` | `"false"` (or unset)
- **Default**: `"false"` (when unset)
- **Usage**: Set to `"true"` to enable development tools

When enabled (`"true"`), the following additional tools are started:
- **PgWeb**: Web-based PostgreSQL administration tool accessible via Aspire Dashboard
- **RedisInsight**: Visual Redis database management interface
- **Redis Commander**: Alternative Redis management tool

#### Configuration

**Development (automatically set in `launchSettings.json`)**:
```json
{
  "profiles": {
    "https": {
      "environmentVariables": {
        "ASPIRE_INCLUDE_DEV_TOOLS": "true"
      }
    }
  }
}
```

**Production/Docker (recommended)**:
```bash
# Disable development tools
export ASPIRE_INCLUDE_DEV_TOOLS=false
# or simply omit the variable
```

#### Security Considerations

> ⚠️ **Important**: Always set `ASPIRE_INCLUDE_DEV_TOOLS=false` or omit the variable in production environments. Development tools may expose sensitive database information and should only be used in development environments.

## Running the Application

### Prerequisites
- [.NET SDK (latest stable)](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or compatible container runtime
- [Aspire Workload](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)

### Quick Start

1. **Navigate to the AppHost directory**:
   ```bash
   cd src/Aspire/TourBooking.AppHost
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Access the Aspire Dashboard**:
   - Open your browser to the dashboard URL shown in the console
   - View all services, logs, metrics, and traces

### Service Endpoints

After startup, services will be available at dynamically assigned ports. Access them through:

- **Aspire Dashboard**: Primary interface for monitoring and service discovery
- **API Service**: RESTful endpoints for tours and bookings
- **Web Frontend**: Customer-facing application
- **Swagger UI**: API documentation and testing interface

All service URLs are displayed in the Aspire Dashboard for easy access.

### Development Tools (when ASPIRE_INCLUDE_DEV_TOOLS=true)

Development and database management tools are accessible via the Aspire Dashboard:
- **PgWeb**: PostgreSQL database management interface
- **RedisInsight**: Visual Redis database management
- **Redis Commander**: Alternative Redis management tool

## Configuration

### Launch Profiles

The project includes multiple launch profiles in `launchSettings.json`:

- **https**: HTTPS profile with development tools enabled
- **http**: HTTP profile with development tools enabled

Both profiles automatically set `ASPIRE_INCLUDE_DEV_TOOLS=true` for development convenience.

### Resource Dependencies

The AppHost manages the following startup order:

1. **Infrastructure Services**: Redis, PostgreSQL
2. **Migration Service**: Waits for database availability
3. **API Service**: Waits for Redis, database, and migration completion
4. **Web Frontend**: Waits for all dependencies and API service

### Container Persistence

Development infrastructure containers use `ContainerLifetime.Persistent` to maintain data between application restarts:

- PostgreSQL data persists between runs
- Redis data persists between runs
- Observability tool configurations persist

## Observability

The AppHost automatically configures a comprehensive observability stack:

### Metrics
- **Prometheus**: Collects and stores metrics from all services
- **Grafana**: Provides dashboards and visualization

### Logging
- **Aspire Dashboard**: Centralized log viewing and filtering
- **OpenTelemetry**: Structured logging with correlation

### Tracing
- **Jaeger**: Distributed request tracing across services
- **OpenTelemetry**: Automatic trace instrumentation

### Health Checks
- Built-in health check endpoints for all services
- Dependency health monitoring

## Development Workflow

1. **Start the AppHost**: `dotnet run`
2. **Monitor via Dashboard**: Open Aspire Dashboard for real-time monitoring
3. **Access Development Tools**: Use database and cache management tools
4. **View Logs and Traces**: Monitor application behavior in real-time
5. **Test API Endpoints**: Use Swagger UI for API exploration

## Production Considerations

- Set `ASPIRE_INCLUDE_DEV_TOOLS=false` or omit the variable
- Use external, managed databases in production
- Configure proper resource limits and scaling policies
- Implement proper secrets management
- Set up production-grade observability and alerting

## Troubleshooting

### Logs and Diagnostics

- Use the Aspire Dashboard to view real-time logs from all services
- Check individual service health status in the dashboard
- Monitor resource usage and performance metrics
- View distributed traces for request flow analysis

## Related Documentation

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Main Project README](../../../README.md)
- [Architecture Documentation](../../../docs/ARCHITECTURE.md)
- [Development Setup Guide](../../../docs/DEVELOPMENT.md)
