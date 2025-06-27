# Instructions for GitHub Copilot

*Repository-specific guidance for the Bike Tours Booking Platform*

## General

* Make only high confidence suggestions when reviewing code changes.
* Always use the latest stable .NET and C# features including pattern matching, collection expressions, primary constructors, and records.
* Never change global.json unless explicitly asked to.
* Follow Domain-Driven Design principles - keep business logic in domain layer, respect aggregate boundaries.
* Avoid working on more than one file at a time - multiple simultaneous edits cause corruption.
* README.md is the single source of truth for project status and roadmap.
* If you are not sure, do not guess - ask clarifying questions or state that you don't know.
* Verify that generated code is correct and compilable.
* Question any orders that seem to violate best practices, security principles, or could lead to technical debt.
* Suggest improvements when better architectural or technical alternatives exist.
* Before working on any new topic or feature, search available memory for relevant context and prior decisions.

## File Safety & Edit Protocols

### Before Every Edit
1. Check README.md in base directory for current project status and roadmap
2. Identify domain context: Tours/Bookings/Customers/Payments
3. Verify architectural layer: Domain/Application/Infrastructure/API
4. Ensure edit respects aggregate boundaries
5. When committing new or modified .csproj files, verify they don't override settings in Directory.Build.props

### Large File Protocol
When working with large files (>300 lines) or complex changes:
1. Create a detailed plan BEFORE making any edits
2. Get explicit user confirmation before proceeding
3. Focus on one conceptual change at a time
4. Respect aggregate boundaries - never modify multiple aggregates in one edit

### DDD Compliance Checklist
Before every edit, verify:
* Aggregate boundaries respected
* Domain logic stays in domain layer
* No infrastructure concerns in domain
* Value objects remain immutable
* Business rules encapsulated properly

## Formatting

* Apply code-formatting style defined in `.editorconfig`.
* Prefer file-scoped namespace declarations and single-line using directives.
* Insert a newline before the opening curly brace of any code block.
* Use pattern matching and switch expressions wherever possible.
* Use `nameof` instead of string literals when referring to member names.
* Always use explicit braces for all control flow statements.
* For detailed coding standards, see `CODING_GUIDELINES.md`.

### Modern C# Features (Required)

* Use collection expressions for arrays and lists.
* Use primary constructors for dependency injection.
* Use pattern matching and switch expressions wherever possible.
* Use record types for value objects and DTOs.

## Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Building

* We use xUnit with FluentAssertions for testing.
* Organize tests with "Arrange", "Act", "Assert" comments.
* Copy existing style in nearby files for test method names and capitalization.
* Follow the existing test patterns in the corresponding test projects.
* Before building or running tests, ensure proper project setup and dependencies.
* For development setup and testing guidelines, see `docs/DEVELOPMENT.md`.

## Domain-Specific Guidance

### Architectural Layers
* Keep domain logic in the domain layer - no infrastructure concerns in domain entities.
* Application layer coordinates between domain and infrastructure.
* Never access repositories or external services directly from domain entities.
* For detailed architecture patterns and examples, see `docs/ARCHITECTURE.md`.

### Naming Conventions
* Use business-focused class names, avoid generic suffixes like "Handler", "Service", "Manager", "Helper".
* Prefer specific names like `TourBookingProcessor`, `CustomerRegistrationValidator`, `BookingConfirmationSender`.

### Domain Contexts
The project has four main bounded contexts:
* **Tours** - Tour catalog, scheduling, capacity management
* **Bookings** - Reservation system, availability tracking  
* **Customers** - User profiles, authentication, preferences
* **Payments** - Payment processing, pricing, refunds

### File Safety
* Work on only one file at a time to avoid corruption.
* For large files (>300 lines), create a plan before editing and get user confirmation.
* Always check README.md for current project phase before making changes.
* When committing new or modified .csproj files, verify they don't override settings in Directory.Build.props.

## Decision Confidence Levels

### High Confidence - Follow Exactly
* Security practices - never compromise on security
* DDD principles - aggregate boundaries, domain events
* File safety - one file at a time, corruption prevention
* Architecture layers - no infrastructure in domain
* Async patterns - use async/await for I/O operations

### Medium Confidence - Suggest Alternatives
* Performance optimizations - suggest if better approach exists
* Code organization - recommend improvements while following request
* Technology choices - propose alternatives with trade-offs
* Implementation patterns - suggest more maintainable approaches

### Low Confidence - Always Ask for Clarification
* Business requirements - unclear domain logic
* Architecture decisions - major structural changes
* Breaking changes - modifications affecting existing APIs
* Complex trade-offs - multiple valid approaches exist
