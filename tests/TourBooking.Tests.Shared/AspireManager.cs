using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using JetBrains.Annotations;
using Microsoft.Testing.Platform.Services;
using Projects;
using TourBooking.ApiService.Contracts;
using TourBooking.Constants;
using TUnit.Core.Interfaces;

namespace TourBooking.Tests.Shared;

[UsedImplicitly]
[MustDisposeResource]
public sealed class AspireManager : IAsyncInitializer, IAsyncDisposable
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(300);

    public DistributedApplication App { get; private set; } = null!;
    public ApiFixture ApiFixture { get; private set; } = null!;
    public HttpClient ApiHttpClient => ApiFixture.CreateDefaultClient();
    public ToursApiClient ApiClient => new(ApiFixture.CreateDefaultClient());
    public string FrontendEndpoint => App.GetEndpoint(ResourceNames.WebFrontend).ToString().TrimEnd('/');

    public string RedisConnectionString { get; private set; } = null!;
    public string ToursDbConnectionString { get; private set; } = null!;

    public async ValueTask DisposeAsync()
    {
        await App.DisposeAsync();
        await ApiFixture.DisposeAsync()
            .ConfigureAwait(false);
    }

    public async Task InitializeAsync()
    {
        using var cts = new CancellationTokenSource(DefaultTimeout);
        var cancellationToken = cts.Token;

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<TourBooking_AppHost>(cancellationToken);

        App = await appHost.BuildAsync(cancellationToken);
        var resourceNotificationService = App.Services.GetRequiredService<ResourceNotificationService>();
        await App.StartAsync(cancellationToken);

        await resourceNotificationService.WaitForResourceAsync(
            ResourceNames.WebFrontend,
            KnownResourceStates.Running,
            cancellationToken
        ).WaitAsync(cancellationToken);

        RedisConnectionString = await App.GetConnectionStringAsync(ResourceNames.Redis, cancellationToken)
                                ?? throw new InvalidOperationException("No connection string set for Redis");
        ToursDbConnectionString = await App.GetConnectionStringAsync(ResourceNames.ToursDatabase, cancellationToken)
                                  ?? throw new InvalidOperationException("No connection string set for Tours Database");

        Environment.SetEnvironmentVariable($"ConnectionStrings__{ResourceNames.Redis}", RedisConnectionString);
        Environment.SetEnvironmentVariable($"ConnectionStrings__{ResourceNames.ToursDatabase}",
            ToursDbConnectionString);

        ApiFixture = new ApiFixture();
    }
}