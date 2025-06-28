using JetBrains.Annotations;
using Microsoft.Playwright;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public class ContextTest : BrowserTest
{
    public IBrowserContext Context { get; private set; } = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        Context = await NewContext(ContextOptions()).ConfigureAwait(false);
    }

    public virtual BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            Locale = "en-US",
            ColorScheme = ColorScheme.Light,
        };
    }
}