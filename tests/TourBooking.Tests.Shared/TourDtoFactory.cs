using TourBooking.ApiService.Contracts;

namespace TourBooking.Tests.Shared;

public static class TourDtoFactory
{
    public static CreateTourDto Create(
        string? name = null,
        string? description = null,
        decimal? price = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
        => new(
            name ?? "Sample Tour",
            description ?? "A sample tour description",
            price ?? 100.0m,
            startDate ?? DateTime.UtcNow.ToDateOnly(),
            endDate ?? DateTime.UtcNow.ToDateOnly().AddDays(5)
     );
}
