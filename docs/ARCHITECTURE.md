# Architecture Guide

## Clean Architecture Layers

### Domain Layer
The innermost layer containing business logic and domain models. **No dependencies on infrastructure or external concerns.**

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
        await _repository.SaveAsync(this); // No! Domain shouldn't know about persistence
    }
}
```

### Application Layer
Orchestrates business logic, defines interfaces, and implements use cases.

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

### Infrastructure Layer
Implements interfaces defined in Application layer. Contains data access, external services, and technical concerns.

### API Layer
HTTP endpoints, controllers, and external-facing contracts.

## Domain-Driven Design Patterns

### Entities
Rich domain models with business logic:

```csharp
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

### Value Objects
Immutable objects representing concepts without identity:

```csharp
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

### Aggregates
Consistency boundaries that ensure business invariants:

```csharp
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
        // Validation logic
    }
}
```

### Domain Events
Communicate changes between aggregates:

```csharp
// Event definition
public record BookingConfirmedEvent(
    BookingId BookingId,
    CustomerId CustomerId,
    DateTime ConfirmedAt) : IDomainEvent;

// Event handler in application layer
public class BookingConfirmedEventHandler : INotificationHandler<BookingConfirmedEvent>
{
    public async Task Handle(BookingConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Send confirmation email, update read models, etc.
    }
}
```

## Bounded Contexts

The system is divided into four main contexts:

1. **Tours**: Tour catalog, scheduling, capacity management
2. **Bookings**: Reservation system, availability tracking
3. **Customers**: User profiles, authentication, preferences
4. **Payments**: Payment processing, pricing, refunds

Each context maintains its own:
- Domain models
- Application services
- Data persistence
- API endpoints

## Key Principles

1. **Dependencies flow inward**: Outer layers depend on inner layers, never the reverse
2. **Domain isolation**: Business logic is independent of frameworks and infrastructure
3. **Interface segregation**: Define narrow, focused interfaces
4. **Aggregate consistency**: Each aggregate ensures its own invariants
5. **Eventually consistent**: Between aggregates, consistency is eventual via domain events