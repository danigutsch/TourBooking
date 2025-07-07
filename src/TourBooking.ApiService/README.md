# TourBooking.ApiService

A **minimal API service** demonstrating modern ASP.NET Core patterns and best practices. This service manages tour functionality for tour company employees, providing efficient APIs for tour creation and retrieval.

> **Domain Focus**: Internal tour management for company employees and systems.

---

## Minimal APIs Overview

**Minimal APIs** provide a streamlined approach to building HTTP APIs with reduced ceremony and improved performance. Key characteristics:

- **Reduced Overhead**: Faster startup and lower memory usage
- **AOT Compatible**: Native compilation support for optimal performance
- **TypedResults**: Compile-time type safety with automatic OpenAPI metadata
- **Route Groups**: Organized endpoint mapping with shared configuration

### When to Use Minimal APIs

✅ **Best for**: Microservices, cloud-native apps, high-performance scenarios, new projects  
❌ **Consider Controllers for**: Complex model binding, extensive validation requirements, large existing controller codebases

---

## API Documentation

### Interactive Documentation
- **Development**: Swagger UI available when running in development mode
- **OpenAPI Schema**: Available at `/openapi/v1.json` in development

### XML Documentation
Comprehensive API documentation is embedded as XML comments in the source code and automatically included in OpenAPI generation.

---

## Architecture Patterns

This service demonstrates several minimal API best practices:

### Route Groups
```csharp
var toursGroup = app.MapGroup("/tours").WithTags("Tours");
return toursGroup.MapToursEndpoints();
```

### TypedResults
```csharp
private static async Task<Created<GetTourDto>> CreateTour(...)
{
    return TypedResults.Created($"/tours/{tour.Id}", responseDto);
}
```

### Benefits
- **Type Safety**: Compile-time checking and IntelliSense support
- **Auto Documentation**: OpenAPI metadata generated automatically
- **Performance**: AOT-compatible with optimized serialization
- **Testability**: Specific return types for reliable testing

---

## Performance & AOT

Fully **AOT compatible** with optimizations:

```xml
<PropertyGroup>
    <IsAotCompatible>true</IsAotCompatible>
    <EnableRequestDelegateGenerator>true</EnableRequestDelegateGenerator>
</PropertyGroup>
```

**Key Optimizations:**
- `WebApplication.CreateSlimBuilder()` for minimal dependencies
- Custom `JsonSerializerContext` for efficient serialization
- Static endpoint handlers for compile-time optimization

---

## Testing

Comprehensive testing coverage:

- **Integration Tests**: `TourBooking.WebTests` - Full HTTP pipeline with containerized dependencies
- **End-to-End Tests**: `TourBooking.Tests.EndToEnd` - Complete workflows with Playwright
- **Unit Tests**: `TourBooking.Tests.Domain` - Domain logic validation

---

## Quick Start

### Running the Service

**Recommended**: Via .NET Aspire
```bash
dotnet run --project src/TourBooking.AppHost
```

**Standalone**:
```bash
dotnet run --project src/TourBooking.ApiService
```

### Development Workflow
1. Start services via AppHost
2. Access interactive documentation (available in development mode)
3. Use hot reload for rapid development
4. Run tests to verify functionality

---

## Related Projects

### Core Dependencies
- **TourBooking.ApiService.Contracts**: API contracts and typed client
- **TourBooking.Tours.Domain**: Business logic and entities
- **TourBooking.Tours.Application**: Use cases and interfaces
- **TourBooking.Tours.Persistence**: Data access layer

### Infrastructure
- **TourBooking.AppHost**: .NET Aspire orchestration
- **TourBooking.ServiceDefaults**: Common configuration and telemetry

### Testing
- **TourBooking.WebTests**: Integration tests for this service
- **TourBooking.Tests.EndToEnd**: Cross-service workflow validation

---

## Contributing

### Guidelines
- Follow minimal API patterns (route groups, TypedResults, extension methods)
- Maintain AOT compatibility
- Write comprehensive tests (unit, integration, contract)
- Document endpoints with XML comments for OpenAPI generation
- Keep business logic in domain layer

---

*Demonstrates modern .NET minimal API patterns with enterprise-grade quality and performance.*
