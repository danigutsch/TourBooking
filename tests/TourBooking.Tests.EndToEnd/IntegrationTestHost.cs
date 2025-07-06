using Aspire.Hosting;
using JetBrains.Annotations;
using TourBooking.ApiService.Contracts;
using TourBooking.Constants;
using TourBooking.Tests.Shared;
using TUnit.Core.Interfaces;

namespace TourBooking.Tests.EndToEnd;

[UsedImplicitly]
[MustDisposeResource]
public sealed class IntegrationTestHost : IAsyncInitializer, IAsyncDisposable
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(300);

    public DistributedApplication App { get; private set; } = null!;

    public ApiFixture ApiFixture { get; private set; } = null!;
    public HttpClient ApiHttpClient => ApiFixture.CreateDefaultClient();
    public ToursApiClient ApiClient => new(ApiFixture.CreateDefaultClient());

    public WebFixture WebFixture { get; private set; } = null!;
    public string FrontendEndpoint => WebFixture.CreateDefaultClient().BaseAddress!.ToString().TrimEnd('/');

    public string RedisConnectionString { get; private set; } = null!;
    public string ToursDbConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        using var cts = new CancellationTokenSource(DefaultTimeout);
        var cancellationToken = cts.Token;

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TourBooking_AppHost>(cancellationToken);

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
        Environment.SetEnvironmentVariable($"ConnectionStrings__{ResourceNames.ToursDatabase}", ToursDbConnectionString);

        ApiFixture = new ApiFixture();

        var apiServiceHttpEndpoint = App.GetEndpoint(ResourceNames.ApiService, "http");
        var apiServiceHttpsEndpoint = App.GetEndpoint(ResourceNames.ApiService, "https");

        Environment.SetEnvironmentVariable($"services__{ResourceNames.ApiService}__http__0", apiServiceHttpEndpoint.ToString());
        Environment.SetEnvironmentVariable($"services__{ResourceNames.ApiService}__https__0", apiServiceHttpsEndpoint.ToString());

        WebFixture = new WebFixture();
    }

    public async ValueTask DisposeAsync()
    {
        await App.DisposeAsync();
        await ApiFixture.DisposeAsync()
            .ConfigureAwait(false);
    }
}
