using JetBrains.Annotations;
using Microsoft.Playwright;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public class BrowserTest : PlaywrightTest
{
    private readonly List<IBrowserContext> _contexts = [];
    public IBrowser Browser { get; internal set; } = null!;

    /// <summary>
    ///     Creates a new context and adds it to the list of contexts to be disposed.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<IBrowserContext> NewContext(BrowserNewContextOptions? options = null)
    {
        var context = await Browser.NewContextAsync(options).ConfigureAwait(false);
        _contexts.Add(context);
        return context;
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        var service = await BrowserService.Register(this, BrowserType).ConfigureAwait(false);
        Browser = service.Browser;
    }

    public override async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (TestOk)
        {
            foreach (var context in _contexts)
            {
                await context.CloseAsync().ConfigureAwait(false);
            }
        }

        _contexts.Clear();
        Browser = null!;
        await base.DisposeAsync().ConfigureAwait(false);
    }
}