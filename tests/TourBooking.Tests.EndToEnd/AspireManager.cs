using Aspire.Hosting;
using JetBrains.Annotations;
using TourBooking.Aspire.Constants;

namespace TourBooking.Tests.EndToEnd;

[UsedImplicitly]
public sealed class AspireManager : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(120);

    public DistributedApplication App { get; private set; } = null!;
    public HttpClient ApiClient { get; private set; } = null!;
    public string FrontendEndpoint { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        using var cts = new CancellationTokenSource(DefaultTimeout);
        var cancellationToken = cts.Token;

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TourBooking_AppHost>(cancellationToken);

        App = await appHost.BuildAsync(cancellationToken);
        var resourceNotificationService = App.Services.GetRequiredService<ResourceNotificationService>();
        await App.StartAsync(cancellationToken);
        ApiClient = App.CreateHttpClient(ResourceNames.WebFrontend);
        await resourceNotificationService.WaitForResourceAsync(
            ResourceNames.WebFrontend,
            KnownResourceStates.Running,
            cancellationToken
            ).WaitAsync(cancellationToken);

        FrontendEndpoint = App.GetEndpoint(ResourceNames.WebFrontend).ToString().TrimEnd('/');
    }

    public async Task DisposeAsync() => await (App?.DisposeAsync() ?? ValueTask.CompletedTask);
}
