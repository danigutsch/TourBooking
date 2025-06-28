using JetBrains.Annotations;
using Microsoft.Playwright;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public class PageTest : ContextTest
{
    public IPage Page { get; private set; } = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        Page = await Context.NewPageAsync().ConfigureAwait(false);
    }
}