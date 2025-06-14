using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TourBooking.Aspire.Constants;
using TourBooking.Tours.Application;

namespace TourBooking.Tours.Persistence;

/// <summary>
/// Dependency injection setup for the Tours persistence layer.
/// </summary>
[PublicAPI]
public static class ToursPersistenceDependencyInjection
{
    /// <summary>
    /// Configures persistence services for the Tours module, including database context and repositories.
    /// </summary>
    /// <remarks>This method registers the <see cref="ToursDbContext"/> with a PostgreSQL connection string
    /// named <see cref="ResourceNames.ToursDatabase"/>. It also adds scoped services for <see cref="IUnitOfWork"/> and <see
    /// cref="IToursRepository"/> implementations.</remarks>
    /// <typeparam name="TBuilder">The type of the application builder, which must implement <see cref="IHostApplicationBuilder"/>.</typeparam>
    /// <param name="builder">The application builder used to configure services and database context for the Tours module.</param>
    public static void AddToursPersistenceServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddNpgsqlDbContext<ToursDbContext>(ResourceNames.ToursDatabase);

        builder.Services.TryAddScoped<IUnitOfWork, ToursDbContext>();
        builder.Services.TryAddScoped<IToursRepository, ToursRepository>();
    }

    /// <summary>
    /// Configures the specified <typeparamref name="TBuilder"/> to use the Tours migration manager.
    /// </summary>
    /// <remarks>This method registers the <see cref="ToursDbContext"/> with a PostgreSQL connection string
    /// named <see cref="ResourceNames.ToursDatabase"/>. Additionally, it registers the <see cref="IMigrationManager"/> service with a
    /// scoped lifetime.</remarks>
    /// <typeparam name="TBuilder">The type of the application builder, which must implement <see cref="IHostApplicationBuilder"/>.</typeparam>
    /// <param name="builder">The application builder to configure. Cannot be <see langword="null"/>.</param>
    public static void AddToursMigrationManager<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddNpgsqlDbContext<ToursDbContext>(ResourceNames.ToursDatabase);

        builder.Services.TryAddScoped<IMigrationManager, MigrationManager>();
    }
}
