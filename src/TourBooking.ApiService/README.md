# TourBooking.ApiService

A minimal API service for the Bike Tours Booking Platform, built with ASP.NET Core minimal APIs and organized using route groups for optimal performance and maintainability.

## Minimal APIs Overview

This service demonstrates modern minimal API patterns:

- **Route Groups**: Organizes endpoints with `MapGroup()` for common prefixes and shared configuration
- **Inline Handlers**: Lambda expressions for simple, direct endpoint logic
- **Dependency Injection**: Services injected directly into endpoint handlers
- **TypedResults**: Strongly-typed responses for better tooling and performance
- **Named Endpoints**: Named routes for link generation and testing

## Tours Domain API

### Current Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/tours` | Create a new bike tour |
| `GET` | `/tours` | Retrieve all available tours |

### Domain Features

- **Tour Management**: Create and retrieve bike tour offerings
- **Business Rules**: Tour validation (dates, pricing, capacity)
- **Domain Events**: Automatic event publishing for tour lifecycle changes
- **Clean Architecture**: Separation between API, application, and domain layers
- **Unit of Work**: Transactional consistency across operations

### Tour Properties

- **Name**: Tour title and identifier
- **Description**: Detailed tour information
- **Price**: Tour cost and pricing structure
- **Start/End Dates**: Tour schedule and duration
- **Capacity Management**: Booking limits and availability (planned)

## Technology Stack

- **ASP.NET Core Minimal APIs**: Lightweight HTTP API framework
- **Domain-Driven Design**: Rich domain models with encapsulated business logic
- **OpenAPI/Swagger**: Automatic API documentation
- **Problem Details**: Standardized error responses (RFC 7807)
- **Entity Framework Core**: Data persistence with PostgreSQL

## Quick Start

```bash
# Run via AppHost (recommended)
dotnet run --project src/TourBooking.AppHost

# Access API documentation
https://localhost:7001/swagger
```

## Related Projects

- **TourBooking.ApiService.Contracts**: DTOs, endpoints, and typed client
- **TourBooking.Tours.Domain**: Core business logic and entities
- **TourBooking.Tours.Application**: Use cases and interfaces
- **TourBooking.Tours.Persistence**: Data access and repositories
