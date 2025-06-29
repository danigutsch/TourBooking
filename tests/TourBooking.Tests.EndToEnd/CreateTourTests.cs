using Microsoft.Playwright;
using System.Globalization;
using TUnit.Playwright;

namespace TourBooking.Tests.EndToEnd;

[Category(TestCategories.EndToEnd)]
[NotInParallel("Aspire")]
public sealed class CreateTourTests : PageTest
{
    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }

    [Test]
    public async Task Create_Tour_Page_Is_Reachable()
    {
        // Arrange

        // Act
        await Page.GotoAsync("/create-tour");
        await Page.WaitForSelectorAsync("h1,form");

        // Assert
        var title = await Page.TitleAsync();
        await Assert.That(title).IsEqualTo("Create Tour");
    }

    [Test]
    public async Task Create_Tour_Successfully()
    {
        // Arrange

        // Act
        await Page.GotoAsync("/create-tour");
        await Page.GetByLabel("Name").FillAsync("Amazing Tour");
        await Page.GetByLabel("Description").FillAsync("A wonderful tour");
        await Page.GetByLabel("Price").FillAsync("100.00");
        await Page.GetByLabel("Start Date").FillAsync(DateTime.UtcNow.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
        await Page.GetByLabel("End Date").FillAsync(DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Create Tour" }).ClickAsync();

        // Assert
        await Page.WaitForSelectorAsync(".alert-success");
        var successMessage = await Page.Locator(".alert-success").InnerTextAsync();
        await Assert.That(successMessage).Contains("Tour created successfully", StringComparison.OrdinalIgnoreCase);
    }

    public override BrowserNewContextOptions ContextOptions(TestContext testContext)
    {
        var options = base.ContextOptions(testContext);
        options.BaseURL = Aspire.FrontendEndpoint;
        options.IgnoreHTTPSErrors = true;
        return options;
    }
}
