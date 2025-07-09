# TourBooking Testing Suite

A **comprehensive three-tier testing strategy** implementing modern .NET testing patterns with **TUnit** and **Microsoft Testing Platform**. Features full code coverage analysis, containerized integration testing, and browser automation for enterprise-grade cloud-native applications.

> **Testing Philosophy**: Test-driven development with comprehensive coverage across unit, integration, and end-to-end scenarios.

---

## Quick Start

### Running Tests

```bash
# Run all tests
dotnet test --configuration Release

# Run by category
dotnet test -- --treenode-filter "/*/*/*/*[Category=Unit]" --configuration Release
dotnet test -- --treenode-filter "/*/*/*/*[Category=Integration]" --configuration Release  
dotnet test -- --treenode-filter "/*/*/*/*[Category=EndToEnd]" --configuration Release

# Run with coverage
pwsh ./scripts/run-coverage.ps1 -Configuration Release
```

### Prerequisites for E2E Tests

```bash
# Build solution first
dotnet build

# Install Playwright browsers
pwsh ./tests/TourBooking.Tests.EndToEnd/bin/*/playwright.ps1 install --with-deps
```

> **Requirements**: Docker/Podman for integration tests, Aspire workload for distributed application testing.

---

## Testing Architecture

### Three-Tier Testing Strategy

The testing suite implements a **layered testing approach** that provides comprehensive validation at different levels of the application:

- **Unit Tests**: Fast, isolated domain logic validation
- **Integration Tests**: API and service integration with containerized dependencies  
- **End-to-End Tests**: Complete user workflows with browser automation

### Test Categories

Tests are organized using **dual categorization** defined in `TestCategories.cs`:

**Test Levels** (execution characteristics):
```csharp
public const string Unit = "Unit";
public const string Integration = "Integration"; 
public const string EndToEnd = "EndToEnd";
```

**Bounded Contexts** (domain alignment):
```csharp
public const string Tours = "Tours";
public const string Bookings = "Bookings";
public const string Customers = "Customers";
public const string Payments = "Payments";
```

**Usage Pattern**:
```csharp
[Category(Unit)]
[Category(Tours)]
public class TourTests
{
    // Tests for Tours domain logic
}
```

---

## Test Projects

### Unit Tests (`TourBooking.Tests.Domain`)

**Purpose**: Validates domain logic, business rules, and entity behavior in isolation.

**Key Features**:
- **Fast Execution**: No external dependencies or I/O operations
- **Domain Focus**: Tests business rules, value objects, and aggregate behavior
- **Comprehensive Coverage**: Validates edge cases and business invariants
- **Parameterized Testing**: Data-driven tests for multiple input scenarios

**Example Test Structure**:
```csharp
[Category(Unit)]
public class TourTests
{
    [Test]
    [Arguments(0, 1, 2, 101, 102)]
    public async Task Name_Length_Has_To_Be_Between_3_And_100_Characters(int length)
    {
        // Arrange, Act, Assert pattern
    }
}
```

### Integration Tests (`TourBooking.WebTests`)

**Purpose**: Tests API endpoints and service integration with real infrastructure.

**Key Features**:
- **Containerized Dependencies**: Uses Aspire hosting for realistic testing environment
- **API Contract Validation**: Ensures endpoints meet specification requirements
- **Service Integration**: Tests interactions between application layers
- **Resource Management**: Automatic cleanup and resource disposal

**Infrastructure**:
- **Aspire Manager**: Manages distributed application lifecycle for tests
- **HTTP Client Testing**: Direct API endpoint validation
- **Dependency Injection**: Real service container with test configurations

### End-to-End Tests (`TourBooking.Tests.EndToEnd`)

**Purpose**: Validates complete user workflows through browser automation.

**Key Features**:
- **Playwright Integration**: Browser automation with TUnit.Playwright
- **User Journey Testing**: Complete workflows from user perspective
- **Cross-Browser Support**: Tests across different browser engines
- **Visual Validation**: UI behavior and user interaction testing

**Browser Automation**:
```csharp
[Category(EndToEnd)]
public sealed class TourTests : PageTest
{
    [Test]
    public async Task Get_Tours_Page_Is_Reachable()
    {
        await Page.GotoAsync(ToursWebEndpoints.GetToursPath);
        var title = await Page.TitleAsync();
        await Assert.That(title).IsEqualTo("Tours");
    }
}
```

### Shared Test Utilities (`TourBooking.Tests.Shared`)

**Purpose**: Common testing infrastructure and reusable components.

**Components**:
- **Test Fixtures**: Shared setup and teardown logic
- **Factory Methods**: Test data generation and object creation
- **Extension Methods**: Testing utilities and helper functions
- **Web Fixtures**: HTTP client configuration for testing

---

## Testing Framework

### TUnit Testing Platform

**Modern Testing Framework** with enhanced capabilities:

- **Microsoft Testing Platform**: Latest testing infrastructure
- **Parameterized Tests**: Data-driven testing with multiple inputs
- **Async/Await Support**: Native support for asynchronous operations
- **Flexible Assertions**: Fluent assertion syntax with detailed failure messages
- **Category Filtering**: Organized test execution by category

### Code Coverage

**Comprehensive Coverage Analysis** with automated reporting:

```xml
<PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
```

**Coverage Features**:
- **Multi-Project Coverage**: Analysis across all test projects
- **Filtered Results**: Focuses on application code, excludes external dependencies
- **Multiple Formats**: XML (Cobertura) and HTML report generation
- **CI/CD Integration**: Coverage data for build pipelines

#### Coverage Commands

```bash
# Run all tests with coverage
pwsh ./scripts/run-coverage.ps1 -Configuration Release

# Include end-to-end tests in coverage
pwsh ./scripts/run-coverage.ps1 -Configuration Release -IncludeE2E

# Open coverage report in browser
pwsh ./scripts/run-coverage.ps1 -Configuration Release -OpenReport
```

#### Coverage Results
- **Coverage Reports**: Comprehensive HTML and XML coverage reports generated using ReportGenerator
- **Report Location**: `CoverageReport/index.html`
- **Filtered Assemblies**: Only includes `TourBooking.*` assemblies in coverage analysis, excluding migrations and external dependencies
- **Coverage Formats**: Supports Cobertura XML and HTML reports

#### Creating Coverage Badges
1. **Generate Coverage Report**: `pwsh ./scripts/run-coverage.ps1 -Configuration Release`
2. **Extract Coverage Data**: The script generates badges in `CoverageReport/` directory:
   - `badge_linecoverage.svg` - Line coverage badge
   - `badge_branchcoverage.svg` - Branch coverage badge
   - `badge_combined.svg` - Combined coverage badge
3. **Shields.io Integration**: Use the generated coverage percentage with [Shields.io](https://shields.io/)

---

## Running Tests

### By Test Level

Use TUnit's category filtering to run specific test types:

```bash
# Run unit tests only
dotnet test -- --treenode-filter "/*/*/*/*[Category=Unit]"
```

```bash
# Run integration tests only  
dotnet test -- --treenode-filter "/*/*/*/*[Category=Integration]"
```

```bash
# Run end-to-end tests only
dotnet test -- --treenode-filter "/*/*/*/*[Category=EndToEnd]"
```

### By Bounded Context

Filter tests by domain context:

```bash
# Run all Tours context tests
dotnet test -- --treenode-filter "/*/*/*/*[Category=Tours]"
```

```bash
# Run all Bookings context tests
dotnet test -- --treenode-filter "/*/*/*/*[Category=Bookings]"
```

```bash
# Run all Customers context tests
dotnet test -- --treenode-filter "/*/*/*/*[Category=Customers]"
```

```bash
# Run all Payments context tests
dotnet test -- --treenode-filter "/*/*/*/*[Category=Payments]"
```

### Combined Filtering

Combine categories for precise test selection:

```bash
# Run only Tours unit tests
dotnet test -- --treenode-filter "/*/*/*/*[Category=Unit and Category=Tours]"
```

```bash
# Run Tours integration and end-to-end tests
dotnet test -- --treenode-filter "/*/*/*/*[(Category=Integration or Category=EndToEnd) and Category=Tours]"
```

### All Tests

```bash
# Run all tests
dotnet test --configuration Release

# Run with specific project
dotnet test tests/TourBooking.Tests.Domain --configuration Release
```

### With Coverage

```bash
# Run all tests with coverage analysis
pwsh ./scripts/run-coverage.ps1 -Configuration Release

# Include end-to-end tests in coverage
pwsh ./scripts/run-coverage.ps1 -Configuration Release -IncludeE2E

# Open coverage report
pwsh ./scripts/run-coverage.ps1 -Configuration Release -OpenReport
```

---

## Browser Testing Setup

### Playwright Installation

**Required for End-to-End Tests**: Playwright browsers must be installed before running UI tests.

```bash
# Build solution first (required)
dotnet build

# Install Playwright browsers with dependencies
pwsh ./tests/TourBooking.Tests.EndToEnd/bin/*/playwright.ps1 install --with-deps
```

**Platform Support**: Playwright supports Windows, Linux, and macOS with consistent behavior across platforms.

### Browser Configuration

- **Automatic Installation**: `PlaywrightInstallOnBuild` property handles browser setup
- **Multi-Browser Testing**: Tests can run across Chromium, Firefox, and WebKit
- **Headless Execution**: Optimized for CI/CD environments
- **Debug Support**: Interactive debugging with headed browser mode

---

## Test Data Management

### Factory Patterns

Consistent test data creation through factory methods:

```csharp
// Example from TourDtoFactory
public static CreateTourDto CreateValidTourDto()
{
    return new CreateTourDto
    {
        Name = "Sample Tour",
        Description = "A comprehensive tour description",
        Date = DateTime.UtcNow.AddDays(30).ToDateOnly()
    };
}
```

### Data-Driven Testing

**Parameterized Tests** for comprehensive scenario coverage:
- **Boundary Value Testing**: Edge cases and limits
- **Invalid Input Validation**: Error handling verification  
- **Business Rule Testing**: Domain-specific validation

---

## Dependencies & Infrastructure

### Core Testing Dependencies

- **TUnit**: Modern testing framework with async support
- **Microsoft.Testing.Extensions.CodeCoverage**: Coverage analysis
- **Aspire.Hosting.Testing**: Distributed application testing
- **TUnit.Playwright**: Browser automation integration

### Infrastructure Requirements

- **Container Runtime**: Docker or Podman for integration tests
- **Aspire Workload**: Required for distributed application testing
- **Playwright Browsers**: Installed via scripts for end-to-end tests

---

## Configuration

### Build Configuration

**Microsoft Testing Platform** with optimized settings:
- `UseMicrosoftTestingPlatformRunner`: Enables modern test runner
- `TestingPlatformDotnetTestSupport`: Integration with `dotnet test`
- `OutputType`: Executable for direct test execution

---

## Best Practices

### Test Organization

- **AAA Pattern**: Arrange, Act, Assert structure for clarity
- **Descriptive Names**: Test method names describe the scenario and expected outcome
- **Dual Categorization**: Apply both test level and bounded context categories
- **Resource Management**: Proper disposal and cleanup in test fixtures

**Category Attribution Example**:
```csharp
[Category(Unit)]
[Category(Tours)]
public class TourValidationTests
{
    // Domain-specific unit tests
}

[Category(Integration)]
[Category(Tours)]
public class TourApiTests  
{
    // API integration tests for Tours context
}
```

### Performance Considerations

- **Fast Unit Tests**: No I/O or external dependencies in unit tests
- **Efficient Integration Tests**: Shared test host for integration scenarios
- **Optimized E2E Tests**: Minimal browser operations, focused user journeys

### Maintainability

- **Shared Utilities**: Common functionality in `TourBooking.Tests.Shared`
- **Factory Methods**: Consistent test data creation
- **Configuration Management**: Environment-specific test settings
- **Documentation**: Clear test intent and requirements

---

## Contributing to Tests

1. **Choose Appropriate Level**: Unit, integration, or end-to-end based on scope
2. **Identify Bounded Context**: Tours, Bookings, Customers, or Payments domain
3. **Follow Naming Conventions**: Descriptive method names with scenario context
4. **Apply Dual Categories**: Use both test level and bounded context categories
5. **Include Coverage**: Ensure new functionality is tested comprehensively

**Example Test Attribution**:
```csharp
[Category(Unit)]
[Category(Tours)]
public async Task Tour_Name_Cannot_Be_Empty()
{
    // Test implementation
}
```

### Test Guidelines

- **Test One Thing**: Each test validates a single behavior or requirement
- **Independent Tests**: No dependencies between test methods
- **Deterministic Results**: Tests produce consistent results across runs
- **Clear Assertions**: Specific, meaningful assertion messages

---

## Planned Advanced Testing

- **Performance Testing**: Benchmark critical paths (tour search, booking creation, payment processing)
- **Load Testing**: Stress testing for concurrent bookings and high-traffic scenarios
- **Snapshot Testing**: Ensure API contract consistency and UI component stability over time
- **Mutation Testing**: Awaiting [Stryker.NET Microsoft Testing Platform support](https://github.com/stryker-mutator/stryker-net/issues/3094)
- **Gherkin Behavior Tests**: Business-readable test scenarios

