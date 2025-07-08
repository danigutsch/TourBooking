# TourBooking Testing Suite

A **comprehensive three-tier testing strategy** implementing modern .NET testing patterns with full code coverage analysis. This testing suite demonstrates enterprise-grade testing practices for cloud-native applications using the latest testing frameworks and tools.

> **Testing Philosophy**: Test-driven development with comprehensive coverage across unit, integration, and end-to-end scenarios.

---

## Testing Architecture

### Three-Tier Testing Strategy

The testing suite implements a **layered testing approach** that provides comprehensive validation at different levels of the application:

- **Unit Tests**: Fast, isolated domain logic validation
- **Integration Tests**: API and service integration with containerized dependencies  
- **End-to-End Tests**: Complete user workflows with browser automation

### Test Categories

Tests are organized using standardized categories defined in `TestCategories.cs`:

```csharp
public static class TestCategories
{
    public const string Unit = "Unit";
    public const string Integration = "Integration"; 
    public const string EndToEnd = "EndToEnd";
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

---

## Running Tests

### By Category

Use TUnit's category filtering to run specific test types:

```bash
# Run unit tests only
dotnet test -- --treenode-filter "/*/*/*/*[Category=Unit]"

# Run integration tests only  
dotnet test -- --treenode-filter "/*/*/*/*[Category=Integration]"

# Run end-to-end tests only
dotnet test -- --treenode-filter "/*/*/*/*[Category=EndToEnd]"
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

### Project Structure

```
tests/
├── Directory.Build.props          # Shared MSBuild properties
├── TestCategories.cs              # Test category definitions
├── TourBooking.Tests.Domain/      # Unit tests
├── TourBooking.WebTests/          # Integration tests  
├── TourBooking.Tests.EndToEnd/    # E2E browser tests
└── TourBooking.Tests.Shared/      # Common test utilities
```

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
- **Category Attribution**: All tests properly categorized for filtering
- **Resource Management**: Proper disposal and cleanup in test fixtures

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

### Adding New Tests

1. **Choose Appropriate Level**: Unit, integration, or end-to-end based on scope
2. **Follow Naming Conventions**: Descriptive method names with scenario context
3. **Apply Correct Category**: Use `TestCategories` constants for filtering
4. **Include Coverage**: Ensure new functionality is tested comprehensively

### Test Guidelines

- **Test One Thing**: Each test validates a single behavior or requirement
- **Independent Tests**: No dependencies between test methods
- **Deterministic Results**: Tests produce consistent results across runs
- **Clear Assertions**: Specific, meaningful assertion messages

---

*Demonstrates comprehensive testing strategies with modern .NET testing frameworks and enterprise-grade practices.*
