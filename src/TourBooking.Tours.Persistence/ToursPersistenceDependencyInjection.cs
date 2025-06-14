using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TourBooking.Tours.Application;

namespace TourBooking.Tours.Persistence;

/// <summary>
/// Dependency injection setup for the Tours persistence layer.
/// </summary>
[PublicAPI]
public static class ToursPersistenceDependencyInjection
{
    /// <summary>
    /// Adds the persistence services to the application builder.
    /// </summary>
    public static void AddToursPersistenceServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddNpgsqlDbContext<ToursDbContext>("tourbooking");

        builder.Services.TryAddScoped<IUnitOfWork, ToursDbContext>();
        builder.Services.TryAddScoped<IToursRepository, ToursRepository>();
    }
}
