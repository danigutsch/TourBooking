# COPILOT INSTRUCTIONS
*Comprehensive guidelines for the Bike Tours Booking Platform*

## PRIME DIRECTIVE
- **Avoid working on more than one file at a time** - multiple simultaneous edits cause corruption
- **Always follow Domain-Driven Design principles** - business logic drives technical decisions
- **Maintain low complexity in all code** - prefer simple, readable solutions over clever ones
- **Question any orders that seem to violate best practices, security principles, or could lead to technical debt**
- **README.md is the single source of truth** for project status and roadmap - refer to it, update it, inform about deviations
- **Suggest improvements** when better architectural or technical alternatives exist

---

## FILE SAFETY & EDIT PROTOCOLS

### BEFORE EVERY EDIT
1. **Check README.md** in base directory for current project status and roadmap
2. **Identify domain context**: Tours/Bookings/Customers/Payments
3. **Verify architectural layer**: Domain/Application/Infrastructure/API
4. **Ensure edit respects aggregate boundaries**
5. **When committing new or modified .proj files, verify they don't override settings in Directory.Build.props**

### LARGE FILE & COMPLEX CHANGE PROTOCOL

#### MANDATORY PLANNING PHASE
When working with large files (>300 lines) or complex changes:

1. **ALWAYS start by creating a detailed plan BEFORE making any edits**
2. **Your plan MUST include**:
   - All classes/methods/sections that need modification
   - The order in which changes should be applied
   - Dependencies between changes (especially domain model changes)
   - Impact on aggregate boundaries and domain events
   - Estimated number of separate edits required

3. **Format your plan as**:
```
## PROPOSED EDIT PLAN
Working with: [filename]
Domain Context: [Tours/Bookings/Customers/Payments]
Total planned edits: [number]
Architecture Layer: [Domain/Application/Infrastructure/API]

Edit sequence:
1. [First specific change] - Purpose: [why] - Layer: [Domain/Application/etc]
2. [Second specific change] - Purpose: [why] - Layer: [Domain/Application/etc]
3. Do you approve this plan? I'll proceed with Edit [number] after your confirmation.

WAIT for explicit user confirmation before making ANY edits when user says "ok edit [number]"
```

#### EXECUTION PHASE
- **Focus on one conceptual change at a time**
- **Respect aggregate boundaries** - never modify multiple aggregates in one edit
- **Show clear "before" and "after" snippets** when proposing changes
- **Include concise explanations** of what changed and why
- **Always check if the edit maintains DDD principles**
- **Ensure changes follow C# conventions and .NET best practices**

**After each individual edit, clearly indicate progress**:
```
✅ Completed edit [#] of [total]. Ready for next edit?
```

**If you discover additional needed changes during editing**:
- **STOP and update the plan**
- **Get approval before continuing**
- **Consider if changes require domain event publication**

#### REFACTORING GUIDANCE
When refactoring large files:
- **Break work into logical, independently functional chunks**
- **Ensure each intermediate state maintains functionality**
- **Consider temporary duplication as a valid interim step**
- **Always indicate the refactoring pattern being applied**
- **Respect bounded context boundaries**
- **Maintain aggregate consistency**

#### RATE LIMIT AVOIDANCE
- **For very large files, suggest splitting changes across multiple sessions**
- **Prioritize changes that are logically complete units**
- **Always provide clear stopping points**

### DDD COMPLIANCE CHECKLIST
Before every edit, verify:
- [ ] **Aggregate boundaries respected**
- [ ] **Domain logic stays in domain layer**
- [ ] **No infrastructure concerns in domain**
- [ ] **Value objects remain immutable**
- [ ] **Domain events published when needed**
- [ ] **Business rules encapsulated properly**

### COMPLEXITY GUIDELINES
- **Prefer explicit over implicit**
- **Choose clarity over cleverness**
- **Favor composition over inheritance**
- **Keep methods small and focused**
- **Use meaningful names**
- **Avoid deep nesting**
- **Extract complex logic into well-named methods**
- **Always use explicit braces for control flow statements**

#### Brace Style Guidelines
```csharp
// ✅ Good: Explicit braces for all control flow
public void ProcessBooking(Booking booking)
{
    if (booking == null)
    {
        throw new ArgumentNullException(nameof(booking));
    }
    
    if (booking.IsValid())
    {
        SaveBooking(booking);
    }
    else
    {
        RejectBooking(booking);
    }
}

// ❌ Avoid: Single-line statements without braces
public void ProcessBooking(Booking booking)
{
    if (booking == null)
        throw new ArgumentNullException(nameof(booking));
        
    if (booking.IsValid())
        SaveBooking(booking);
    else
        RejectBooking(booking);
}
```

---

## TECHNICAL STANDARDS & CODE GUIDELINES

### TARGET ENVIRONMENT
- **.NET Version**: 9.0 or higher
- **C# Language Version**: 13 (latest)
- **Nullable Reference Types**: Enabled by default

### MODERN C# FEATURES (REQUIRED)
```csharp
// Use var for local variables when type is obvious
var tourPrice = new TourPrice(150.00m, "USD");
var availableDates = [startDate, endDate, ..middleDates];

// Use record types for value objects and DTOs
public record TourPrice(decimal Amount, string Currency);

// Use primary constructors
public class BookingService(IBookingRepository repository, ILogger<BookingService> logger)
{
    // Implementation
}

// Use collection expressions
var availableDates = [startDate, endDate, ..middleDates];

// Use pattern matching
var result = booking switch
{
    { Status: BookingStatus.Confirmed } => ProcessConfirmed(booking),
    { Status: BookingStatus.Pending } => ProcessPending(booking),
    _ => ProcessDefault(booking)
};
```

### FEATURES TO AVOID
- **Callback-based async patterns** - Use async/await
- **Deep inheritance hierarchies** - Prefer composition
- **Static classes for business logic** - Use proper DI
- **Generic class names** - Avoid "Handler", "Service", "Manager", "Helper" etc.

### NAMING CONVENTIONS

#### Classes and Interfaces
```csharp
// Classes: PascalCase, descriptive business-focused nouns
// ✅ Good: Specific, business-focused names
public class TourBookingService { }
public class CustomerRegistrationHandler { }
public class BookingConfirmationProcessor { }
public class TourAvailabilityCalculator { }

// ❌ Avoid: Generic suffixes without business context
public class BookingHandler { }        // Too generic
public class TourService { }           // What kind of service?
public class CustomerManager { }       // What does it manage?
public class PaymentHelper { }         // Vague purpose

// Interfaces: PascalCase with 'I' prefix, specific contracts
public interface IBookingRepository { }
public interface ITourAvailabilityService { }

// Abstract classes: PascalCase with 'Base' suffix
public abstract class BaseEntity { }
public abstract class BaseCommandHandler { }
```

#### Methods and Properties
```csharp
// Methods: PascalCase, verb-based
public void ProcessBooking() { }
public async Task<Result> ValidatePaymentAsync() { }

// Properties: PascalCase, noun-based
public string CustomerName { get; set; }
public DateTime BookingDate { get; init; }

// Private fields: camelCase with underscore
private readonly ILogger _logger;
private readonly BookingSettings _settings;
```

#### Constants and Enums
```csharp
// Constants: PascalCase
public const int MaxBookingsPerCustomer = 10;
public static readonly TimeSpan BookingTimeout = TimeSpan.FromMinutes(15);

// Enums: PascalCase for both enum and values
public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}
```

### CODE ORGANIZATION

#### File Structure
```csharp
// File-scoped namespaces (required)
namespace BikeToursBooking.Domain.Tours;

// Using statements order:
// 1. System namespaces
// 2. Third-party packages  
// 3. Project namespaces
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using BikeToursBooking.Domain.Common;
```

#### Class Organization
```csharp
public class TourBooking : BaseEntity
{
    // 1. Constants
    public const int MaxParticipants = 20;
    
    // 2. Fields
    private readonly List<Participant> _participants = [];
    
    // 3. Constructors
    public TourBooking(TourId tourId, CustomerId customerId) { }
    
    // 4. Properties
    public TourId TourId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    
    // 5. Public methods
    public void AddParticipant(Participant participant) { }
    
    // 6. Private methods
    private void ValidateParticipantLimit() { }
}
```

### DOMAIN-DRIVEN DESIGN PATTERNS

#### Entities
```csharp
// Rich domain models with business logic
public class Tour : BaseEntity
{
    private readonly List<Booking> _bookings = [];
    
    public TourId Id { get; private set; }
    public string Name { get; private set; }
    public int MaxCapacity { get; private set; }
    
    public void AddBooking(Booking booking)
    {
        if (_bookings.Count >= MaxCapacity)
        {
            throw new TourCapacityExceededException();
        }
            
        _bookings.Add(booking);
        RaiseDomainEvent(new BookingAddedEvent(Id, booking.Id));
    }
}
```

#### Value Objects
```csharp
// Immutable records with validation
public record Email(string Value)
{
    public Email(string value) : this(value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            throw new ArgumentException("Invalid email format");
        }
    }
}

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);
    public Money Add(Money other) => 
        Currency == other.Currency 
            ? new(Amount + other.Amount, Currency)
            : throw new InvalidOperationException("Currency mismatch");
}
```

#### Aggregates
```csharp
// Consistency boundaries
public class BookingAggregate : BaseEntity
{
    private readonly List<BookingItem> _items = [];
    
    // All modifications go through the aggregate root
    public void AddItem(TourId tourId, DateTime date, int participants)
    {
        ValidateBookingRules(tourId, date, participants);
        var item = new BookingItem(tourId, date, participants);
        _items.Add(item);
        RaiseDomainEvent(new BookingItemAddedEvent(Id, item.Id));
    }
    
    // Business rules enforced here
    private void ValidateBookingRules(TourId tourId, DateTime date, int participants)
    {
        // Implementation
    }
}
```

#### Domain Events
```csharp
// Immutable records for events
public record BookingConfirmedEvent(
    BookingId BookingId,
    CustomerId CustomerId,
    DateTime ConfirmedAt) : IDomainEvent;

// Event handlers in application layer
public class BookingConfirmedEventHandler : INotificationHandler<BookingConfirmedEvent>
{
    public async Task Handle(BookingConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Send confirmation email, update read models, etc.
    }
}
```

### CLEAN ARCHITECTURE LAYERS

#### Domain Layer Rules
```csharp
// ✅ Good: Pure business logic
public class Tour
{
    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0)
        {
            throw new InvalidTourPriceException();
        }
        Price = newPrice;
    }
}

// ❌ Bad: Infrastructure concerns in domain
public class Tour
{
    public async Task UpdatePriceAsync(decimal amount)
    {
        // Don't access database or external services here
        await _repository.SaveAsync(this); // ❌ No!
    }
}
```

#### Application Layer Patterns
```csharp
// CQRS Commands
public record CreateTourCommand(
    string Name,
    string Description,
    decimal Price,
    int MaxCapacity) : ICommand<Result<TourId>>;

// Command Handlers
public class CreateTourCommandHandler : ICommandHandler<CreateTourCommand, Result<TourId>>
{
    public async Task<Result<TourId>> Handle(CreateTourCommand command, CancellationToken cancellationToken)
    {
        // Validation, business logic coordination, persistence
    }
}

// Queries
public record GetAvailableToursQuery(
    DateTime StartDate,
    DateTime EndDate,
    int MinCapacity) : IQuery<List<TourDto>>;
```

### ERROR HANDLING

#### Exception Strategy
```csharp
// Domain exceptions for business rule violations
public class TourCapacityExceededException : DomainException
{
    public TourCapacityExceededException(int requestedCapacity, int maxCapacity)
        : base($"Requested capacity {requestedCapacity} exceeds maximum {maxCapacity}")
    {
    }
}

// Result pattern for operation outcomes
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string Error { get; private set; }
    
    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
}
```

#### Async/Await Patterns
```csharp
// ✅ Good: Proper async implementation
public async Task<Result<Booking>> CreateBookingAsync(CreateBookingCommand command)
{
    try
    {
        var tour = await _tourRepository.GetByIdAsync(command.TourId);
        if (tour is null)
        {
            return Result<Booking>.Failure("Tour not found");
        }
            
        var booking = tour.CreateBooking(command.CustomerId, command.Participants);
        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<Booking>.Success(booking);
    }
    catch (DomainException ex)
    {
        return Result<Booking>.Failure(ex.Message);
    }
}
```

### TESTING GUIDELINES

#### Unit Test Structure
```csharp
// Arrange-Act-Assert pattern
[Fact]
public void AddBooking_WhenCapacityExceeded_ShouldThrowException()
{
    // Arrange
    var tour = new Tour("Mountain Bike Adventure", maxCapacity: 1);
    var firstBooking = new Booking(CustomerId.New(), 1);
    var secondBooking = new Booking(CustomerId.New(), 1);
    tour.AddBooking(firstBooking);
    
    // Act & Assert
    var exception = Assert.Throws<TourCapacityExceededException>(
        () => tour.AddBooking(secondBooking));
        
    exception.Message.Should().Contain("capacity");
}
```

### DOCUMENTATION STANDARDS

#### XML Documentation
```csharp
/// <summary>
/// Creates a new tour booking for the specified customer and tour.
/// </summary>
/// <param name="customerId">The customer making the booking.</param>
/// <param name="tourId">The tour being booked.</param>
/// <param name="participants">Number of participants.</param>
/// <returns>Result with the created booking or error.</returns>
public async Task<Result<Booking>> CreateBookingAsync(
    CustomerId customerId, 
    TourId tourId, 
    int participants)
{
    // Implementation
}
```

### PERFORMANCE GUIDELINES

#### Entity Framework Patterns
```csharp
// ✅ Good: Use AsNoTracking for read-only queries
public async Task<List<TourDto>> GetAvailableToursAsync()
{
    return await _context.Tours
        .AsNoTracking()
        .Where(t => t.AvailableSpots > 0)
        .Select(t => new TourDto(t.Id, t.Name, t.Price))
        .ToListAsync();
}

// ✅ Good: Use Include for related data
public async Task<Tour> GetTourWithBookingsAsync(TourId id)
{
    return await _context.Tours
        .Include(t => t.Bookings)
        .FirstOrDefaultAsync(t => t.Id == id);
}
```

#### Caching Patterns
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

---

## PROJECT MANAGEMENT & COORDINATION

### README MANAGEMENT PROTOCOL

#### Core Principles
- **README.md is the single source of truth** for project status and roadmap
- **Always refer to README.md** in base directory for project reference and implementation roadmap
- **Inform immediately** if any request contradicts guidelines or deviates from planned implementation phases
- **If you insist on deviating** from the README plan, update the README accordingly to reflect the new direction
- **Update the README whenever features are implemented** to reflect current project status and mark completed items

#### When to Update README
1. **Feature Implementation**: Change `[ ]` to `[x]` for completed items
2. **Plan Deviation**: Update roadmap when changing direction (with user insistence)
3. **New Features Added**: Add to appropriate phase when scope expands
4. **Phase Completion**: Update status and move to next phase
5. **Architecture Changes**: Update technical stack or approach descriptions

#### README Update Format
```
## IMPLEMENTATION STATUS UPDATE
- **Phase**: [Current phase]
- **Completed**: [What was finished]
- **Updated Status**: [Specific checkbox changes]
- **Next Priority**: [What's next in roadmap]
```

### SUGGESTION PROTOCOL

#### When to Suggest Improvements
- **Better architectural patterns** for the proposed solution
- **More appropriate design patterns** for the use case
- **Performance optimizations** that don't add complexity
- **Security improvements** that should be considered
- **Maintainability enhancements** aligned with DDD principles
- **Technology alternatives** that better fit the requirements

#### How to Suggest Improvements
```
## ALTERNATIVE APPROACH
**Current request**: [Summarize what was asked]
**Potential issue**: [What could be improved]
**Suggested alternative**: [Better approach]
**Benefits**: [Why it's better]
**Trade-offs**: [Any downsides to consider]

Do you want to proceed with the original approach or explore the alternative?
```

### PROJECT STRUCTURE REQUIREMENTS

The project follows Clean Architecture principles with clear separation of concerns across Domain, Application, Infrastructure, and API layers. Each domain context (Tours, Bookings, Customers, Payments) maintains its own bounded context within the architecture.

### IMPLEMENTATION PHASES REFERENCE

#### Phase 1: Core Foundation
- Clean Architecture setup
- Domain-Driven Design implementation
- CQRS pattern foundation
- Event-driven architecture
- .NET Aspire cloud-native setup
- PostgreSQL persistence
- Redis distributed caching
- Basic authentication
- OpenAPI/Swagger documentation
- Unit testing suite
- Integration tests with containers
- Health checks
- OpenTelemetry observability
- Problem Details error handling

#### Phase 2: Core Business Features
- Tour catalog and search functionality
- Booking system with real-time availability
- Basic payment processing
- Customer registration and profiles
- Tour guide assignment
- Basic email notifications
- Tour aggregates and entities
- Booking and customer domains
- Payment and pricing models
- Basic inventory management

#### Phase 3: Advanced Features
- Payment gateway integration (Stripe, PayPal)
- Multi-currency support
- Tour recommendations engine
- Customer reviews and ratings
- Event Sourcing
- Real-time updates (SignalR)
- Full-text search for tours

#### Phase 4: Enterprise Features
- Advanced security (OAuth2, RBAC)
- Weather integration
- Route planning and GPS integration
- Cloud deployment
- Performance optimization
- Monitoring dashboards

### WORKFLOW PROTOCOLS

#### Starting New Features
1. **Check README phase alignment**
2. **Verify prerequisites are completed**
3. **Confirm domain context and bounded context**
4. **Plan architectural approach**
5. **Get approval before implementation**

#### Feature Completion
1. **Update README status**
2. **Verify tests pass**
3. **Update documentation**
4. **Check for breaking changes**
5. **Consider impact on dependent features**

#### Conflict Resolution
1. **Identify the conflict** (guidelines vs request vs README)
2. **Present all options** with trade-offs
3. **Recommend best approach** based on principles
4. **Wait for user decision**
5. **Update documentation** if direction changes

### COMMUNICATION STANDARDS

#### Progress Updates
- Always indicate current phase and progress
- Reference README section being worked on
- Highlight any blockers or dependencies
- Suggest next logical steps

#### Decision Points
- Present clear options with pros/cons
- Reference relevant guidelines or README sections
- Recommend preferred approach with reasoning
- Wait for explicit approval before proceeding

---

## CONTEXT DETECTION & DECISION MAKING

### DECISION CONFIDENCE LEVELS

#### 🔴 HIGH CONFIDENCE - Follow Exactly
- **Security practices** - never compromise on security
- **DDD principles** - aggregate boundaries, domain events
- **File safety** - one file at a time, corruption prevention
- **Architecture layers** - no infrastructure in domain
- **Async patterns** - use async/await for I/O operations

#### 🟡 MEDIUM CONFIDENCE - Suggest Alternatives
- **Performance optimizations** - suggest if better approach exists
- **Code organization** - recommend improvements while following request
- **Technology choices** - propose alternatives with trade-offs
- **Implementation patterns** - suggest more maintainable approaches

#### 🟢 LOW CONFIDENCE - Always Ask for Clarification
- **Business requirements** - unclear domain logic
- **Architecture decisions** - major structural changes
- **Breaking changes** - modifications affecting existing APIs
- **Complex trade-offs** - multiple valid approaches exist

### CONTEXT DETECTION TRIGGERS

#### Keywords That Require Extra Scrutiny
- **"security", "auth", "password", "token"** → Apply maximum security standards
- **"performance", "slow", "optimization"** → Reference performance guidelines
- **"database", "migration", "schema"** → Verify EF Core patterns
- **"test", "testing", "coverage"** → Ensure comprehensive testing approach
- **"deploy", "production", "release"** → Check production readiness

#### File Type Specific Actions
- **`.csproj` files** → Verify Directory.Build.props alignment
- **`*Controller.cs`, `*Endpoint.cs`** → Apply API layer guidelines
- **`*Repository.cs`** → Verify repository pattern compliance
- **`*Entity.cs`, `*Aggregate.cs`** → Enforce DDD principles
- **`*Test.cs`, `*Tests.cs`** → Apply testing standards

#### Size-Based Protocols
- **>300 lines** → Mandatory planning phase with explicit approval
- **>500 lines** → Suggest file splitting or refactoring
- **>1000 lines** → Strong recommendation to break into multiple sessions

#### Domain Context Detection
- **Tours namespace/folder** → Tour catalog, scheduling, capacity management
- **Bookings namespace/folder** → Reservation system, availability tracking
- **Customers namespace/folder** → User profiles, authentication, preferences
- **Payments namespace/folder** → Payment processing, pricing, refunds

---

## QUICK REFERENCE

### Technology Stack (Current Implementation Phase):
- **.NET 9** with **C# 13** features
- **Clean Architecture** + **Domain-Driven Design**
- **PostgreSQL** + **Entity Framework Core 9**
- **Redis** for distributed caching
- **.NET Aspire** for cloud-native orchestration
- **xUnit** + **FluentAssertions** for testing

### Domain Contexts:
- **Tours** - Tour catalog, scheduling, capacity management
- **Bookings** - Reservation system, availability tracking
- **Customers** - User profiles, authentication, preferences
- **Payments** - Payment processing, pricing, refunds

### Before Starting Any Work:
1. ✅ Check README.md for current project phase and status
2. ✅ Apply file safety protocols (one file at a time)
3. ✅ Verify DDD compliance requirements
4. ✅ Plan large changes with explicit approval
5. ✅ Use appropriate modern C# 13 features and patterns

### Emergency Stops:
- **Security violations** - Stop and challenge
- **Multiple file edits** - Risk of corruption
- **Domain boundary violations** - Maintain aggregate integrity
- **Architecture layer mixing** - Keep concerns separated
- **Breaking changes without approval** - Get confirmation first

---

**📋 This comprehensive guide contains all protocols, standards, and guidelines for the Bike Tours Booking Platform development. Follow these instructions consistently to maintain code quality, architectural integrity, and project coherence.**