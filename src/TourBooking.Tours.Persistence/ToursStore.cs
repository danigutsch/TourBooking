using Microsoft.EntityFrameworkCore;
using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;

namespace TourBooking.Tours.Persistence;

internal sealed class ToursStore(ToursDbContext context) : IToursStore
{
    public void Add(Tour tour) => context.Set<Tour>().Add(tour);
    public async Task<IReadOnlyList<Tour>> GetAll(CancellationToken ct) => await context.Set<Tour>().ToListAsync(ct).ConfigureAwait(false);
}
