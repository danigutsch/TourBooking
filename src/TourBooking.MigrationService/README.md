# TourBooking.MigrationService

A **database migration service** that runs database schema and data migrations before other services start. This service ensures that the database is in the correct state for the application to function properly.

## Overview

The Migration Service is a **background service** that executes database migration scripts in a controlled manner. It runs as part of the .NET Aspire orchestration and completes before other application services start, ensuring data consistency and schema compatibility across deployments.

### Key Features

- **Pre-startup Execution**: Runs migrations before application services start
- **SQL Script Processing**: Executes SQL migration scripts from a configured directory
- **Observability**: Full OpenTelemetry integration with tracing and logging
- **Fail-fast Behavior**: Stops application startup if migrations fail
- **Extensible Design**: Built to support multiple migration strategies and databases

## Architecture

The service follows a clean separation of concerns:

- **`Migrator`**: Background service that orchestrates the migration process
- **`IMigrationManager`**: Interface for executing migration scripts (implemented in persistence layer)
- **`MigrationManager`**: Implementation that handles SQL script execution via Entity Framework

## Configuration

### Required Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `TOURS_MIGRATIONS_PATH` | Absolute path to directory containing SQL migration scripts | `/app/migrations/tours` |

### Migration Scripts

- SQL files (`.sql` extension) placed in the configured migrations directory
- Scripts are executed in **descending alphabetical order** (latest script first)
- Currently executes only the latest migration script
- Empty or missing scripts will cause the service to fail

## Usage

### In .NET Aspire AppHost

```csharp
var migrationService = builder.AddProject<Projects.TourBooking_MigrationService>("migration-service")
    .WithEnvironment("TOURS_MIGRATIONS_PATH", "/app/migrations/tours");

var apiService = builder.AddProject<Projects.TourBooking_ApiService>("api-service")
    .WaitFor(migrationService); // Ensures migrations run first
```

### Migration Script Structure

Place SQL migration scripts in the configured directory:

```
migrations/
└── tours/
    ├── 001_initial_schema.sql
    ├── 002_add_booking_tables.sql
    └── 003_add_indexes.sql
```

## Implementation Details

### Service Lifecycle

1. **Startup**: Service starts as a background service
2. **Validation**: Checks that migrations directory exists and contains SQL files
3. **Script Selection**: Identifies the latest migration script (alphabetically last)
4. **Execution**: Runs the migration script via `IMigrationManager`
5. **Completion**: Stops the application host after successful migration

### Error Handling

- **Missing Directory**: Throws `InvalidOperationException` if migrations path doesn't exist
- **No Scripts**: Throws `InvalidOperationException` if no `.sql` files found
- **Empty Script**: Throws `InvalidOperationException` if script content is empty
- **Execution Errors**: Database errors are propagated and logged with full tracing

### Observability

- **Activity Source**: `TourBooking.MigrationService` for distributed tracing
- **Structured Logging**: Comprehensive logging with event IDs for monitoring
- **Status Tracking**: OpenTelemetry activities track success/failure status

## Development

### Testing

The service can be tested by:
1. Configuring a test migrations directory
2. Placing test SQL scripts in the directory
3. Running the service with appropriate configuration

### Extending Migration Logic

Additional migration capabilities can be implemented in the `MigrationManager` class:
- Version tracking
- Rollback support
- Multi-database support
- Migration validation

## Dependencies

- **TourBooking.Tours.Application**: Migration manager interface
- **TourBooking.Tours.Persistence**: Migration manager implementation
- **TourBooking.ServiceDefaults**: Telemetry and service configuration
- **.NET Aspire**: Service orchestration and dependency management

## Future Enhancements

- Support for multiple database contexts
- Migration version tracking and rollback capabilities
- Parallel migration execution for independent scripts
- Pre and post-migration validation hooks
- Support for data seeding scripts
