using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourBooking.ApiService.Contracts;
using TourBooking.Tours.Persistence;

namespace TourBooking.Tests.Shared;

/// <summary>
/// Base class for tour-related integration tests providing common setup.
/// </summary>
[PublicAPI]
public abstract class TourTestBase : IDisposable
{
    private readonly CancellationTokenSource _cts = new(TimeSpan.FromSeconds(30));

    private bool _disposed;
    private IServiceScope _scope = null!;
    private ToursDbContext _toursContext = null!;

    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }

    protected CancellationToken Token => _cts.Token;
    protected ToursApiClient ApiClient => Aspire.ApiClient;
    protected HttpClient ApiHttpClient => Aspire.ApiHttpClient;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected DbSet<T> Set<T>() where T : class => _toursContext.Set<T>();

    [Before(Test)]
    public void BeforeTest()
    {
        _scope = Aspire.ApiFixture.Services.CreateScope();
        _toursContext = _scope.ServiceProvider.GetRequiredService<ToursDbContext>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _cts.Dispose();
            _scope.Dispose();
            _toursContext.Dispose();
        }

        _disposed = true;
    }
}