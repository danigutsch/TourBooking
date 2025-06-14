using TourBooking.Tours.Application;
using TourBooking.Tours.Domain;

namespace TourBooking.Tours.Persistence;

internal sealed class ToursRepository(ToursDbContext context) : IToursRepository
{
    public void Add(Tour tour) => context.Set<Tour>().Add(tour);
}
