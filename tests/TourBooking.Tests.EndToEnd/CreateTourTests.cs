using System.Globalization;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace TourBooking.Tests.EndToEnd;

[Collection("Aspire")]
public sealed class CreateTourTests(AspireManager aspire) : PageTest, IClassFixture<AspireManager>
{
    private readonly string _frontendEndpoint = aspire.FrontendEndpoint;

    [Fact]
    public async Task Create_Tour_Page_Is_Reachable()
    {
        // Arrange

        // Act
        await Page.GotoAsync($"{_frontendEndpoint}/create-tour");
        await Page.WaitForSelectorAsync("h1,form");

        // Assert
        var title = await Page.TitleAsync();
        Assert.Equal("Create Tour", title);
    }

    [Fact]
    public async Task Create_Tour_Successfully()
    {
        // Arrange

        // Act
        await Page.GotoAsync($"{_frontendEndpoint}/create-tour");

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

    public override BrowserNewContextOptions ContextOptions()
    {
        var options = base.ContextOptions();
        options.BaseURL = _frontendEndpoint;
        return options;
    }
}
