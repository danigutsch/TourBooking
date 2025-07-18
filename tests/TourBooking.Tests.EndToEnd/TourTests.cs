using Microsoft.Playwright;
using System.Globalization;
using TourBooking.Web.Contracts;
using TUnit.Playwright;

namespace TourBooking.Tests.EndToEnd;

[Category(TestCategories.EndToEnd)]
public sealed class TourTests : PageTest
{
    [ClassDataSource<IntegrationTestHost>(Shared = SharedType.PerTestSession)]
    public required IntegrationTestHost IntegrationTestHost { get; init; }

    [Test]
    public async Task Get_Tours_Page_Is_Reachable()
    {
        // Arrange

        // Act
        await Page.GotoAsync(ToursWebEndpoints.GetToursPath);

        // Assert
        var title = await Page.TitleAsync();
        await Assert.That(title).IsEqualTo("Tours");
    }

    [Test]
    public async Task Get_Tours_List_Is_Populated()
    {
        // Arrange

        // Act
        await Page.GotoAsync(ToursWebEndpoints.GetToursPath);
        await Page.WaitForSelectorAsync("table");

        // Assert
        var rows = await Page.Locator("table tbody tr").CountAsync();
        await Assert.That(rows).IsGreaterThan(0);
    }

    [Test]
    public async Task Create_Tour_Page_Is_Reachable()
    {
        // Arrange

        // Act
        await Page.GotoAsync(ToursWebEndpoints.CreateTourPath);
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
        await Page.GotoAsync(ToursWebEndpoints.CreateTourPath);
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
        options.BaseURL = IntegrationTestHost.FrontendEndpoint;
        options.IgnoreHTTPSErrors = true;
        return options;
    }
}
