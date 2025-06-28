using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace TourBooking.Tests.EndToEnd.Playwright;

internal sealed class BrowserService : IWorkerService
{
    public IBrowser Browser { get; }

    private BrowserService(IBrowser browser) => Browser = browser;

    public Task DisposeAsync() => Browser.CloseAsync();
    public Task ResetAsync() => Task.CompletedTask;

    public static Task<BrowserService> Register(WorkerAwareTest test, IBrowserType browserType) =>
        test.RegisterService(
            "Browser",
            async () => new BrowserService(await CreateBrowser(browserType).ConfigureAwait(false)));

    private static async Task<IBrowser> CreateBrowser(IBrowserType browserType)
    {
        var accessToken = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_ACCESS_TOKEN");
        var serviceUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_URL");

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(serviceUrl))
        {
            return await browserType.LaunchAsync(PlaywrightSettingsProvider.LaunchOptions).ConfigureAwait(false);
        }

        var exposeNetwork = Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_EXPOSE_NETWORK") ?? "<loopback>";
        var os = Uri.EscapeDataString(Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_OS") ?? "linux");
        var runId = Uri.EscapeDataString(
            Environment.GetEnvironmentVariable("PLAYWRIGHT_SERVICE_RUN_ID") ??
#pragma warning disable RS0030
            DateTime.Now.ToUniversalTime().ToString(
                "yyyy-MM-ddTHH:mm:ss",
                CultureInfo.InvariantCulture));
#pragma warning restore RS0030
        const string apiVersion = "2023-10-01-preview";
        var wsEndpoint = $"{serviceUrl}?os={os}&runId={runId}&api-version={apiVersion}";
        var connectOptions = new BrowserTypeConnectOptions
        {
            Timeout = 3 * 60 * 1000,
            ExposeNetwork = exposeNetwork,
            Headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {accessToken}",
                ["x-playwright-launch-options"] = JsonSerializer.Serialize(PlaywrightSettingsProvider.LaunchOptions,
                    JsonSerializerOptions),
            }
        };

        return await browserType.ConnectAsync(wsEndpoint, connectOptions).ConfigureAwait(false);
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}