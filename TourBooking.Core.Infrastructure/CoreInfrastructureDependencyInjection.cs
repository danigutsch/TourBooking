using Microsoft.Extensions.Hosting;
using TourBooking.Aspire.Constants;

namespace TourBooking.Core.Infrastructure;

/// <summary>
/// Provides extension methods for registering core infrastructure services in an application.
/// </summary>
/// <remarks>This class contains methods to simplify the registration of essential infrastructure services, such
/// as caching, for applications using the <see cref="IHostApplicationBuilder"/> interface.</remarks>
public static class CoreInfrastructureDependencyInjection
{
    /// <summary>
    /// Adds core infrastructure services to the specified application builder.
    /// </summary>
    /// <remarks>This method configures the application builder to use Redis as the output cache,  with the
    /// Redis instance identified by the resource name <see cref="ResourceNames.Redis"/>.</remarks>
    /// <typeparam name="TBuilder">The type of the application builder, which must implement <see cref="IHostApplicationBuilder"/>.</typeparam>
    /// <param name="builder">The application builder to which the core infrastructure services will be added.</param>
    public static void AddCoreInfrastructureServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddRedisOutputCache(ResourceNames.Redis);
    }
}
