using JetBrains.Annotations;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public class PlaywrightTest : WorkerAwareTest
{
    public string BrowserName { get; internal set; } = null!;

    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowserType BrowserType { get; private set; } = null!;

    private static readonly Task<IPlaywright> _playwrightTask = Microsoft.Playwright.Playwright.CreateAsync();

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        Playwright = await _playwrightTask.ConfigureAwait(false);
        BrowserName = PlaywrightSettingsProvider.BrowserName;
        BrowserType = Playwright[BrowserName];
        Playwright.Selectors.SetTestIdAttribute("data-testid");
    }

    public static void SetDefaultExpectTimeout(float timeout)
    {
        Assertions.SetDefaultExpectTimeout(timeout);
    }

#pragma warning disable CA1822
#pragma warning disable S2325
    public ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);

    public IPageAssertions Expect(IPage page) => Assertions.Expect(page);

    public IAPIResponseAssertions Expect(IAPIResponse response) => Assertions.Expect(response);
#pragma warning restore CA1822
#pragma warning restore S2325
}