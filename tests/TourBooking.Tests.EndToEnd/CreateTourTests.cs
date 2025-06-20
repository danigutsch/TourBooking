using Microsoft.Playwright.Xunit;
using TourBooking.Aspire.Constants;

namespace TourBooking.Tests.EndToEnd;

public sealed class CreateTourTests : PageTest
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task Create_Tour_Page_Is_Reachable()
    {
        // Arrange
        using var cts = new CancellationTokenSource(DefaultTimeout);
        var cancellationToken = cts.Token;

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TourBooking_AppHost>(cancellationToken);

        await using var app = await appHost.BuildAsync(cancellationToken);
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync(cancellationToken);
        using var httpClient = app.CreateHttpClient(ResourceNames.WebFrontend);
        await resourceNotificationService.WaitForResourceAsync(ResourceNames.WebFrontend, KnownResourceStates.Running, cancellationToken).WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
        var endpoint = app.GetEndpoint(ResourceNames.WebFrontend).ToString().TrimEnd('/');

        // Act
        await Page.GotoAsync($"{endpoint}/create-tour");
        await Page.WaitForSelectorAsync("h1,form");

        // Assert
        var title = await Page.TitleAsync();
        Assert.Equal("Create Tour", title);
    }
}
