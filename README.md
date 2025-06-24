[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Linux/Windows/macOS](https://img.shields.io/badge/platform-linux--windows--macos-blue)](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)

# Bike Tours Booking Platform

A **cloud-native bike tours booking and management** system built with the **latest stable .NET and C# features** as they become available, showcasing modern language capabilities, **Domain-Driven Design (DDD)**, and **clean architecture** principles. This repository demonstrates how to build a **production-ready distributed** application that focuses on scalability, maintainability, and exceptional customer experience in cloud-native environments.

> **🚧 Project Status**: This project is in early development. Core architecture, infrastructure, and initial APIs are scaffolded. See the roadmap for progress.

---

## Table of Contents
1. [Overview](#overview)  
2. [Core Features](#core-features)  
3. [Architecture](#architecture)  
4. [Implementation Roadmap](#implementation-roadmap)  
5. [Getting Started](#getting-started)  
6. [Development](#development)  
7. [Contributing](#contributing)  
8. [License](#license)

---

## Overview

The **Bike Tours Booking Platform API** provides a comprehensive framework for managing bike tours, customer bookings, inventory, and e-commerce operations. Leveraging the **latest stable .NET and .NET Aspire** ecosystem, this project employs enterprise architectural patterns to ensure high availability, scalability, and testability for a seamless customer booking experience in cloud-native environments.

### Key Benefits
- **Modern .NET Stack**: Built with the latest stable C# features as they become available
- **Cloud-Native Design**: Optimized for containerized deployment and microservices
- **Enterprise Patterns**: Implements DDD, CQRS, and event-driven architecture
- **Production Ready**: Comprehensive testing, monitoring, and observability
- **Developer Experience**: Hot reload, dev containers, and comprehensive tooling

---

## Core Features

- **Tour Lifecycle Management**: Complete management of bike tours from creation to completion  
- **Customer Booking System**: Seamless reservation system with real-time availability  
- **E-commerce Integration**: Secure payment processing and order management  
- **Inventory Management**: Bike availability, maintenance tracking, and resource allocation  
- **Customer Management**: User profiles, booking history, and preference tracking  
- **Real-time Updates**: Live availability and booking confirmations
- **Comprehensive APIs**: RESTful APIs with GraphQL and gRPC support
- **Multi-tenant Architecture**: Support for multiple tour operators

---

## Architecture

### Clean Architecture Layers
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

## Implementation Roadmap

### 🏗️ **Architecture & Design**
- [x] Clean Architecture implementation
- [x] Domain-Driven Design patterns
- [x] CQRS pattern
- [ ] Event-driven architecture
- [ ] Event Sourcing
- [ ] Outbox pattern
- [ ] Read/write segregation
- [ ] Multi-tenancy support
- [ ] Binary data handling
- [ ] Efficient media file processing
- [ ] Document storage patterns

### 🏗️ **Infrastructure**

#### Cloud & Hosting
- [x] .NET Aspire cloud-native setup
- [ ] Basic authentication
- [ ] Full-text search capabilities
- [ ] Azure App Service / AWS Elastic Beanstalk

#### Persistence
**SQL Databases**
- PostgreSQL
  - [x] Local
  - [ ] Azure Database for PostgreSQL
  - [ ] Amazon RDS for PostgreSQL
- SQLite
  - [ ] Local
  - [ ] Azure SQL Edge
  - [ ] AWS Aurora Serverless
- [ ] Database maintenance
- Data migration tools
  - [x] Local (EF Core Migrations)
  - [ ] Flyway
  - [ ] Liquibase
  - [ ] Azure Data Factory
  - [ ] AWS DMS
- SQL Server
  - [ ] Azure SQL Database
  - [ ] Amazon RDS for SQL Server

**NoSQL & Document Storage**
- Document DB
  - [ ] Local (e.g., MongoDB)
  - [ ] Azure Cosmos DB
  - [ ] Amazon DynamoDB
- [ ] Document storage patterns

#### Caching Systems
**Memory Caching**
- [ ] .NET In-Memory Cache
- [ ] Output caching
- [ ] Response caching

**Distributed Caching**
- Redis
  - [ ] Local
  - [ ] Azure Cache for Redis
  - [ ] Amazon ElastiCache
- [ ] Garnet caching integration

**Hybrid & Advanced**
- [ ] Hybrid caching system
- [ ] Cache invalidation strategies

#### APIs & Endpoints
**REST APIs**
- [x] Controller-based APIs
- [ ] Minimal APIs
- [ ] OpenAPI/Swagger documentation
- [ ] API versioning

**Alternative Protocols**
- [ ] gRPC services (Azure API Management, AWS App Mesh)
- [ ] GraphQL endpoint (Azure API Management, AWS AppSync)

### ⚡ **Performance & Optimization**

#### Compilation & Runtime
- [ ] AOT compilation
- [ ] JIT optimization
- [ ] Native AOT support

#### Response & Output
- [ ] Response compression
- [ ] Output compression
- [ ] Static file optimization

#### Database & Queries
- [ ] Query optimization
- [ ] Lazy loading implementation
- [ ] Connection pooling
- [ ] Database indexing strategies

#### Processing & Scaling
- [ ] Batch processing system
- [ ] Background job processing
- [ ] Parallel processing
- [ ] Resource management

### 🧪 **Testing**
- [ ] Unit testing suite
- [ ] Integration tests with containers
- [x] UI tests with Playwright
- [ ] Mutation testing
- [ ] Gherkin behavior tests

### ✅ **Quality Assurance**
- [x] Health checks
- [x] OpenTelemetry observability
- [ ] Problem Details error handling
- [ ] Feature flags & toggles
- [ ] Snapshot testing
- [ ] Multi-configuration testing
- [ ] AI integration for testing
- [ ] Performance benchmarks
- [ ] Distributed tracing

### 🔗 **Communication & Integration**

#### Real-time Communication
- [ ] Real-time updates (SignalR)
- [ ] WebSocket connections
- [ ] Server-sent events

#### Message Systems
- [ ] Message broker integration
- [ ] Data streaming (Kafka)
- [ ] Event bus implementation
- [ ] Message queuing

#### External Integrations
- [ ] Third-party payment processors
- [ ] External API integrations
- [ ] Webhook support
- [ ] OAuth provider integrations

#### Notifications & Alerts
- [ ] Email notifications
- [ ] SMS notifications
- [ ] Push notifications
- [ ] In-app notifications

#### Frontend Integration
- [ ] Front-end frameworks integration
- [ ] SPA routing support
- [ ] CORS configuration

### 💾 **Storage & Data Management**

#### File Storage
- [ ] S3 storage integration
- [ ] Local file storage
- [ ] CDN integration
- [ ] File upload handling

#### Media Processing
- [ ] Image optimization
- [ ] Video processing
- [ ] Binary data handling
- [ ] Efficient media file processing

#### Data Operations
- [ ] Data archival/retention
- [ ] Data compression
- [ ] Data validation
- [ ] Data transformation

#### Multi-tenancy & Partitioning
- [ ] Storage tenant system
- [ ] Data partitioning
- [ ] Tenant isolation
- [ ] Cross-tenant queries

#### Backup & Recovery
- [ ] Backup automation
- [ ] Point-in-time recovery
- [ ] Disaster recovery
- [ ] Data synchronization

### 🔐 **Security & Authentication**

#### Authentication & Authorization
- [ ] Bearer token authentication
- [ ] OpenID Connect (OIDC)
- [ ] OAuth2
- [ ] SSO (Single Sign-On)
- [ ] Social media sign-on (Google, Facebook, Microsoft, Apple, GitHub, etc.)
- [ ] Self-built authentication
- [ ] Keycloak integration
- [ ] Duende Identity Server
- [ ] Azure AD / AWS Cognito integration

#### Security Controls
- [ ] Rate limiting
- [ ] Request idempotency
- [ ] Secrets management (Azure Key Vault, AWS Secrets Manager)
- [ ] API key management
- [ ] Audit logging
- [ ] Data encryption (Azure Managed HSM, AWS KMS)
- [ ] GDPR compliance
- [ ] Role-based file access
- [ ] Security headers
- [ ] CORS configuration

### 🛠️ **DevOps & Deployment**

#### Infrastructure & Automation
- [ ] Cloud deployment templates
- [ ] Blue-green deployments
- [ ] Infrastructure as Code
- [ ] Monitoring dashboards
- [ ] Backup strategies
- [ ] Database maintenance
- [ ] Production migration practices
- [ ] Auto semantic versioning in CI
- [ ] NuGet package distribution
- [ ] Project templates

#### Containerization & CI/CD
- [ ] Development containers (.devcontainer)
- [ ] Docker Compose setup
- [ ] Kubernetes manifests
- [ ] Helm charts
- [ ] CI/CD pipelines
- [ ] Trimmed .NET runtime images (Alpine, distroless, Aspire runtime-deps)
- [ ] Multi-arch container images (ARM64/x64)
- [ ] Container image vulnerability scanning
- [ ] SBOM (Software Bill of Materials) generation
- [ ] Image signing and verification
- [ ] Multi-stage Docker builds for minimal images
- [ ] Non-root container user setup
- [ ] Container image hardening and attack surface reduction
- [ ] Docker Slim or similar image minimization tools

### 📚 **Documentation & Knowledge**

#### Architecture & Design Docs
- [ ] Domain model diagrams
- [ ] Bounded context map
- [ ] Aggregate relationship diagrams
- [ ] Event flow documentation
- [ ] Architecture decision records (ADRs)
- [ ] Domain description and glossary

#### API & Deployment Docs
- [ ] API documentation
- [ ] Deployment guides
- [ ] Development setup guides

### 🎯 **Business Features**

#### Core Business
- [ ] Seasonal tour scheduling  
- [ ] Dynamic pricing algorithms
- [ ] Tax calculation  
- [ ] Subscription billing for memberships  
- [ ] Cancellation policies  
- [ ] AI integration for recommendations  
- [ ] Customer loyalty programs
- [ ] Review and rating system

#### Internationalization & Operations
- [ ] Multi-language support
- [ ] Currency conversion
- [ ] Weather integration
- [ ] Route optimization
- [ ] Equipment management
- [ ] Tour guide scheduling

---

## Getting Started

### Prerequisites
- **Supported OS:** Linux, Windows, macOS
- [.NET SDK (latest stable)](https://dotnet.microsoft.com/)  
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman](https://podman.io/) (**FOSS alternative**) or compatible container runtime (**required for Aspire orchestration and cloud-native development**)
- [Visual Studio 2022](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/) with C# extension, or [JetBrains Rider](https://www.jetbrains.com/rider/)
- [Aspire Workload & Tooling](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling) (**required to run AppHost**)

> This project is cross-platform and can be developed and run on Linux, Windows, and macOS. All features and tooling are supported on these operating systems.

> For more information on .NET Aspire, see the [official documentation](https://learn.microsoft.com/en-us/dotnet/aspire/).

### Quick Start

1. **Clone the repository**:
   ```bash
   git clone https://github.com/danigutsch/BikeToursBooking.git
   cd BikeToursBooking
   ```

2. **Restore development tools**:
   ```bash
   dotnet tool restore
   ```

3. **Build the solution** (required before Playwright browser install):
   ```bash
   dotnet build
   ```

4. **Install Playwright browsers (required for end-to-end UI tests)**:
   ```bash
   pwsh ./tests/TourBooking.Tests.EndToEnd/bin/*/playwright.ps1 install --with-deps
   ```
   > Use the wildcard (*) to match your build configuration and .NET version (e.g., Debug/net9.0 or Release/net9.0). For details, see the [Playwright .NET docs](https://playwright.dev/dotnet/docs/intro).

5. **Run the application**:
   ```bash
   dotnet run --project src/Aspire/TourBooking.AppHost
   ```

6. **Access the application**:
   - API: `https://localhost:7001`
   - Swagger UI: `https://localhost:7001/swagger`
   - Aspire Dashboard: `https://localhost:15888`

7. **Run all tests**:
   ```bash
   dotnet test --configuration Release
   ```
   - To run only unit tests: `dotnet test --filter "Category=Unit" --configuration Release`
   - To run integration/API tests: `dotnet test tests/TourBooking.WebTests --configuration Release`
   - To run end-to-end UI tests: `dotnet test tests/TourBooking.Tests.EndToEnd --configuration Release`

> Aspire-based tests require Docker or a compatible container runtime running.
> Playwright browsers must be installed before running E2E tests.

---

## Development

### Project Structure

```
src/
├── Aspire/
│   ├── TourBooking.AppHost/           # .NET Aspire orchestration and cloud infra
│   └── TourBooking.Aspire.Constants/  # Aspire resource constants
├── TourBooking.ApiService/            # Web API endpoints and controllers
├── TourBooking.ApiService.Contracts/  # API contracts
├── TourBooking.Core.Infrastructure/   # Infrastructure and dependency injection
├── TourBooking.MigrationService/      # Database migration logic
├── TourBooking.ServiceDefaults/       # Service defaults and extensions
├── TourBooking.Tours.Application/     # Application layer (CQRS, interfaces)
├── TourBooking.Tours.Domain/          # Domain models and logic
├── TourBooking.Tours.Persistence/     # Persistence layer (EF Core, repositories)
└── TourBooking.Web/                   # Customer-facing web application

tests/
└── TourBooking.WebTests/              # Web layer tests

grafana/
otelcollector/
prometheus/
```

### Key Design Principles

- **Domain-First Approach**: Business logic drives technical decisions
- **Eventual Consistency**: Distributed data consistency through domain events
- **Fail-Fast Validation**: Early input validation with comprehensive error handling
- **Immutable Value Objects**: Ensuring data integrity and thread safety
- **Rich Domain Models**: Business rules encapsulated within domain entities
- **Separation of Concerns**: Clear boundaries between layers and responsibilities
- **Cloud-Native Patterns**: Built for scalability and resilience
- **Developer Experience**: Fast feedback loops and productive tooling

---

## Contributing

We welcome contributions! Please follow these guidelines:

### Getting Started

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
- Run code analysis and formatting tools

### Code Standards

- Follow C# coding conventions
- Use meaningful names for variables, methods, and classes
- Write XML documentation for public APIs
- Implement comprehensive error handling
- Follow SOLID principles

### Pull Request Process

1. Update the README.md with details of changes if applicable
2. Update the version numbers following semantic versioning
3. Ensure the PR description clearly describes the problem and solution
4. Include any relevant issue numbers

---

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT) - see the LICENSE file for details.

---
