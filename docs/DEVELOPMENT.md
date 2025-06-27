# Development Guide

## Prerequisites

- Latest stable .NET SDK
- Docker Desktop or Podman (for Aspire orchestration)
- Visual Studio 2022, VS Code with C# extension, or JetBrains Rider
- Aspire Workload & Tooling

## Project Structure

```
src/
├── Aspire/
│   ├── TourBooking.AppHost/           # .NET Aspire orchestration
│   └── TourBooking.Aspire.Constants/  # Aspire resource constants
├── TourBooking.ApiService/            # Web API endpoints
├── TourBooking.ApiService.Contracts/  # API contracts
├── TourBooking.Core.Infrastructure/   # Infrastructure and DI
├── TourBooking.MigrationService/      # Database migrations
├── TourBooking.ServiceDefaults/       # Service defaults
├── TourBooking.Tours.Application/     # Application layer (CQRS)
├── TourBooking.Tours.Domain/          # Domain models and logic
├── TourBooking.Tours.Persistence/     # Persistence layer (EF Core)
└── TourBooking.Web/                   # Customer-facing web app

tests/
├── TourBooking.Tests.Domain/          # Domain unit tests
├── TourBooking.Tests.EndToEnd/        # E2E tests with Playwright
└── TourBooking.WebTests/              # Integration tests
```

## Technology Stack

- **Runtime**: Latest .NET and C#
- **Cloud**: .NET Aspire for cloud-native orchestration
- **Database**: PostgreSQL with Entity Framework Core
- **Caching**: Redis
- **Testing**: xUnit v3, Microsoft.Testing.Platform, Playwright
- **Monitoring**: OpenTelemetry, Prometheus, Grafana, Jaeger

## Development Setup

### 1. Clone and Restore
```bash
git clone https://github.com/danigutsch/BikeToursBooking.git
cd BikeToursBooking
dotnet tool restore
```

### 2. Build Solution
```bash
dotnet build
```

### 3. Install Test Dependencies
```bash
# Install Playwright browsers for E2E tests
pwsh ./tests/TourBooking.Tests.EndToEnd/bin/*/playwright.ps1 install --with-deps
```

### 4. Run Application
```bash
dotnet run --project src/Aspire/TourBooking.AppHost
```

### 5. Access Services
- API: `https://localhost:7001`
- Swagger UI: `https://localhost:7001/swagger`
- Aspire Dashboard: `https://localhost:15888`

## Testing

### Run All Tests
```bash
dotnet test --configuration Release
```

### Run Specific Test Categories
```bash
# Unit tests only
dotnet test --filter "Category=Unit" --configuration Release

# Integration tests
dotnet test tests/TourBooking.WebTests --configuration Release

# E2E tests
dotnet test tests/TourBooking.Tests.EndToEnd --configuration Release
```

### Quarantined Tests
Tests marked with `[QuarantinedTest("issue-url")]` are flaky and excluded from regular runs. They run in the outerloop workflow instead.

## Entity Framework Commands

### Add Migration
```bash
dotnet ef migrations add MigrationName --project src/TourBooking.Tours.Persistence --startup-project src/TourBooking.MigrationService
```

### Update Database
```bash
dotnet ef database update --project src/TourBooking.Tours.Persistence --startup-project src/TourBooking.MigrationService
```

## Performance Guidelines

### Entity Framework Patterns
```csharp
// Use AsNoTracking for read-only queries
public async Task<List<TourDto>> GetAvailableToursAsync()
{
    return await _context.Tours
        .AsNoTracking()
        .Where(t => t.AvailableSpots > 0)
        .Select(t => new TourDto(t.Id, t.Name, t.Price))
        .ToListAsync();
}

// Use Include for related data
public async Task<Tour> GetTourWithBookingsAsync(TourId id)
{
    return await _context.Tours
        .Include(t => t.Bookings)
        .FirstOrDefaultAsync(t => t.Id == id);
}
```

### Caching Patterns
```csharp
// Use IMemoryCache for frequently accessed data
public async Task<Tour> GetTourAsync(TourId id)
{
    var cacheKey = $"tour-{id}";
    
    if (_cache.TryGetValue(cacheKey, out Tour cachedTour))
    {
        return cachedTour;
    }
        
    var tour = await _repository.GetByIdAsync(id);
    _cache.Set(cacheKey, tour, TimeSpan.FromMinutes(10));
    
    return tour;
}
```

## Debugging Tips

### Aspire Dashboard
- View all running services and their logs
- Monitor HTTP requests between services
- Check health status of dependencies

### Docker Containers
```bash
# View running containers
docker ps

# View container logs
docker logs <container-name>

# Access container shell
docker exec -it <container-name> /bin/bash
```

### Common Issues

**Port Conflicts**
- Check if ports 7001, 15888, or database ports are in use
- Modify `launchSettings.json` if needed

**Container Issues**
- Ensure Docker Desktop is running
- Try `docker system prune` to clean up resources

**Certificate Errors**
- Run `dotnet dev-certs https --trust`
- On Linux/Mac, additional steps may be required

## Environment Variables

Key environment variables used:
- `ASPNETCORE_ENVIRONMENT`: Development/Staging/Production
- `DOTNET_ENVIRONMENT`: Development/Staging/Production
- `ConnectionStrings__tourbooking`: Database connection
- `Redis__ConnectionString`: Redis connection

## Monitoring and Observability

### OpenTelemetry
- Traces: Jaeger UI at `http://localhost:16686`
- Metrics: Prometheus at `http://localhost:9090`
- Dashboards: Grafana at `http://localhost:3000`

### Health Checks
- API health: `/health`
- Detailed health: `/health/detail`

## Deployment

See deployment guides in the `docs/deployment/` folder for:
- Local Docker deployment
- Azure deployment
- AWS deployment
- Kubernetes deployment