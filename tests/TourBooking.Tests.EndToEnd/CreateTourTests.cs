using System.Globalization;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using TourBooking.Aspire.Constants;

namespace TourBooking.Tests.EndToEnd;

[Collection("Aspire")]
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

    [Fact]
    public async Task Create_Tour_Successfully()
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

        await Page.GetByLabel("Name").FillAsync("Amazing Tour");
        await Page.GetByLabel("Description").FillAsync("A wonderful tour");
        await Page.GetByLabel("Price").FillAsync("100.00");
        await Page.GetByLabel("Start Date").FillAsync(DateTime.UtcNow.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
        await Page.GetByLabel("End Date").FillAsync(DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Create Tour" }).ClickAsync();

        // Assert
        await Page.WaitForSelectorAsync(".alert-success");
        var successMessage = await Page.Locator(".alert-success").InnerTextAsync();
        Assert.Contains("Tour created successfully", successMessage, StringComparison.OrdinalIgnoreCase);
    }
}
