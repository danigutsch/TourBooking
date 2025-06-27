# Coding Guidelines

## Language Features
- Use the latest stable C# and .NET features
- Target the latest .NET version
- Nullable reference types are enabled by default

## Modern C# Features (Required)

### Type Inference and Collections
```csharp
// Use var for local variables when type is obvious
var tourPrice = new TourPrice(150.00m, "USD");
var availableDates = [startDate, endDate, ..middleDates];

// Use collection expressions
var dates = [date1, date2, date3]; // not new[] { date1, date2, date3 }
```

### Record Types and Primary Constructors
```csharp
// Use record types for value objects and DTOs
public record TourPrice(decimal Amount, string Currency);

// Use primary constructors
public class BookingService(IBookingRepository repository, ILogger<BookingService> logger)
{
    // Implementation
}
```

### Pattern Matching
```csharp
// Prefer pattern matching over if-else chains
var result = booking switch
{
    { Status: BookingStatus.Confirmed } => ProcessConfirmed(booking),
    { Status: BookingStatus.Pending } => ProcessPending(booking),
    _ => ProcessDefault(booking)
};
```

## Naming Conventions

### Classes and Interfaces
```csharp
// Classes: PascalCase, descriptive business-focused nouns
public class TourBookingProcessor { }           // ✅ Specific
public class CustomerRegistrationHandler { }    // ✅ Domain-focused

// Avoid generic suffixes without context
public class BookingHandler { }     // ❌ Too generic
public class TourService { }        // ❌ What kind of service?

// Interfaces: PascalCase with 'I' prefix
public interface IBookingRepository { }
public interface ITourAvailabilityService { }

// Abstract classes: PascalCase with 'Base' suffix
public abstract class BaseEntity { }
```

### Methods and Properties
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

### Constants and Enums
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

## Code Organization

### File Structure
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

### Class Organization
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

## Control Flow and Braces
Always use explicit braces for all control flow statements:

```csharp
// ✅ Good: Explicit braces
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

// ❌ Avoid: Single-line without braces
if (booking == null)
    throw new ArgumentNullException(nameof(booking));
```

## Async/Await Patterns
```csharp
// ✅ Good: Proper async implementation
public async Task<Result<Booking>> CreateBookingAsync(CreateBookingCommand command)
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

// ❌ Avoid: Blocking async calls
var result = SomeAsyncMethod().Result;  // Never do this
SomeAsyncMethod().Wait();               // Or this
```

## XML Documentation
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

## Features to Avoid
- Callback-based async patterns - Use async/await
- Deep inheritance hierarchies - Prefer composition
- Static classes for business logic - Use proper DI
- `.Result` or `.Wait()` on tasks - Use async/await
- Single-line statements without braces