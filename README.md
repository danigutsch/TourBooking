[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Bike Tours Booking Platform

A **cloud-native bike tours booking and management** system built with **.NET 9** and **.NET Aspire**, showcasing modern C# features, **Domain-Driven Design (DDD)**, and **clean architecture** principles. This repository demonstrates how to build a **production-ready distributed** application that focuses on scalability, maintainability, and exceptional customer experience in cloud-native environments.

---

## Table of Contents
1. [Overview](#overview)  
2. [Core Features](#core-features)  
3. [Architecture](#architecture)  
    - [Clean Architecture](#clean-architecture)  
    - [Domain-Driven Design](#domain-driven-design)  
4. [Technical Stack](#technical-stack)  
    - [Implemented](#implemented)  
    - [Planned](#planned)  
5. [Getting Started](#getting-started)  
    - [Prerequisites](#prerequisites)  
    - [Running the Application](#running-the-application)  
    - [Test Credentials](#test-credentials)  
6. [Development](#development)  
    - [Project Structure](#project-structure)  
    - [Key Design Principles](#key-design-principles)  
    - [Infrastructure Configurations](#infrastructure-configurations)  
    - [Package Distribution](#package-distribution)  
7. [Contributing](#contributing)  
8. [License](#license)

---

## Overview

> **🚧 Project Status**: This is a new project currently in the planning and initial development phase. All features listed below are planned for implementation.

The **Bike Tours Booking Platform API** will provide a comprehensive framework for managing bike tours, customer bookings, inventory, and e-commerce operations. Leveraging **.NET 9** and the **.NET Aspire** ecosystem, this project will employ enterprise architectural patterns to ensure high availability, scalability, and testability for a seamless customer booking experience in cloud-native environments.

---

## Core Features

- **Tour Lifecycle Management**: Complete management of bike tours from creation to completion  
- **Customer Booking System**: Seamless reservation system with real-time availability  
- **E-commerce Integration**: Secure payment processing and order management  
- **Inventory Management**: Bike availability, maintenance tracking, and resource allocation  
- **Customer Management**: User profiles, booking history, and preference tracking  
- **Distributed Caching**: Improves performance for tour searches and availability checks with Redis  
- **Event-Driven Updates**: Ensures consistent state and real-time availability across microservices  
- **Comprehensive Testing**: Includes unit tests, integration tests, and health checks  

---

## Architecture

### Clean Architecture
- **Domain**: Core business models and logic for tours, bookings, and customers  
- **Application**: Use cases and interfaces for booking workflows and tour management  
- **Infrastructure**: Technical implementations (persistence, payment gateways, caching, external services)  
- **API**: HTTP endpoints and contracts for customer and admin interfaces  

### Domain-Driven Design
- **Bounded Contexts**: Logical separation of Tours, Bookings, Customers, and Payments sub-domains  
- **Rich Domain Models**: Business rules and invariants for tour scheduling, capacity, and pricing  
- **Aggregate Roots**: Transactional consistency boundaries for bookings and tour management  
- **Domain Events**: Publish/subscribe mechanism for booking confirmations, payment processing, and notifications  
- **Value Objects**: Immutable data types for tour dates, locations, pricing, and customer information  
- **Ubiquitous Language**: Consistent language for domain experts, tour operators, and developers  

---

## Technical Stack

### Implementation Status

This is a **brand new project** - all features below are planned for implementation.

### Phase 1: Core Foundation

**Documentation & Architecture**
- [ ] Domain model diagrams  
- [ ] Bounded context map  
- [ ] Architecture decision records (ADRs)  
- [ ] Domain description and glossary  

**Advanced Patterns & Performance**
- [ ] Read/write segregation  
- [ ] Multi-tenancy (for tour operators)  
- [ ] Garnet caching integration  
- [ ] AOT compilation  
- [ ] Response compression  
- [ ] Query optimization  

**Storage & Media**
- [ ] S3 storage for tour media  
- [ ] Binary data handling for tour images/videos  
- [ ] Document storage for booking confirmations  
- [ ] Data streaming (Kafka)  
- [ ] Data archival/retention  

**Additional Business Features**
- [ ] Seasonal tour scheduling  
- [ ] Tax calculation  
- [ ] Subscription billing for memberships  
- [ ] Cancellation policies  
- [ ] AI integration for recommendations  
- [ ] Feature flags & toggles  

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/)  
- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- Entity Framework Core tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Running the Application

> **Note**: This is a new project in development. The following steps will be available once the initial implementation is complete.

1. **Clone the repository**:
   ```bash
   git clone https://github.com/danigutsch/BikeToursBooking.git
   cd BikeToursBooking
   ```

2. **Start the infrastructure services**:
   ```bash
   docker-compose up -d
   ```

3. **Update the database**:
   ```bash
   dotnet ef database update --project src/Infrastructure --startup-project src/API
   ```

4. **Run the application**:
   ```bash
   dotnet run --project src/AppHost
   ```

5. **Access the application** *(planned endpoints)*:
   - **API Documentation**: `https://localhost:7001/swagger`
   - **Customer Portal**: `https://localhost:7002`
   - **Admin Dashboard**: `https://localhost:7003`
   - **.NET Aspire Dashboard**: `https://localhost:15000`

### Test Credentials *(Planned)*
**Customer Account**:
- Email: `customer@biketours.com`
- Password: `Customer123!`

**Tour Operator Account**:
- Email: `operator@biketours.com`
- Password: `Operator123!`

**Admin Account**:
- Email: `admin@biketours.com`
- Password: `Admin123!`

---

## Development

### Project Structure *(Planned)*
```
src/
├── API/                     # Web API endpoints and controllers
├── Application/             # Use cases, CQRS handlers, and interfaces
├── Domain/                  # Core business logic and domain models
├── Infrastructure/          # Data persistence, external services, caching
├── AppHost/                 # .NET Aspire orchestration
└── Web/                     # Customer-facing web application

tests/
├── UnitTests/               # Domain and application layer tests
├── IntegrationTests/        # API and infrastructure tests
└── FunctionalTests/         # End-to-end booking scenarios
```

### Key Design Principles
- **Domain-First Approach**: Business logic drives technical decisions
- **Eventual Consistency**: Distributed data consistency through domain events
- **Fail-Fast Validation**: Early input validation with comprehensive error handling
- **Immutable Value Objects**: Ensuring data integrity and thread safety
- **Rich Domain Models**: Business rules encapsulated within domain entities
- **Separation of Concerns**: Clear boundaries between layers and responsibilities

### Infrastructure Configurations
- **PostgreSQL**: Primary data store with Entity Framework Core
- **Redis**: Distributed caching for tour availability and session management
- **OpenTelemetry**: Comprehensive observability and performance monitoring
- **Health Checks**: Application and dependency health monitoring
- **Problem Details**: Standardized API error responses

### Package Distribution
The project is structured to support NuGet package distribution for:
- **Domain Models**: Reusable business entities and value objects
- **Application Contracts**: Shared interfaces and DTOs
- **Infrastructure Components**: Common caching, logging, and data access patterns

---

## Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository** and create a feature branch
2. **Follow the coding standards** established in the project
3. **Write comprehensive tests** for new functionality
4. **Update documentation** for any API changes
5. **Submit a pull request** with a clear description of changes

### Development Workflow
- Use **conventional commits** for clear commit messages
- Ensure all **tests pass** before submitting PRs
- Follow **domain-driven design** principles for new features
- Maintain **backward compatibility** for public APIs

---

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT) - see the LICENSE file for details.

---
