using System.Text.Json;
using TourBooking.Tests;
using TourBooking.Web.Contracts;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
public sealed class ApiTests
{
    [ClassDataSource<AspireManager>(Shared = SharedType.PerTestSession)]
    public required AspireManager Aspire { get; init; }

    [Test]
    public async Task OpenApi_Endpoint_Is_Reachable()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var httpClient = Aspire.ApiHttpClient;

        // Act
        var response = await httpClient.GetAsync(ToursWebEndpoints.OpenApi, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");

        var jsonContent = await response.Content.ReadAsStringAsync(TestContext.Current?.CancellationToken ?? cts.Token);
        await Assert.That(jsonContent).IsNotNull().And.IsNotEmpty();
        
        var openApiDoc = JsonDocument.Parse(jsonContent);
        var root = openApiDoc.RootElement;
        
        await Assert.That(root.TryGetProperty("openapi", out var openApiVersion)).IsTrue();
        await Assert.That(openApiVersion.GetString()).IsNotNull().And.StartsWith("3.");
        
        await Assert.That(root.TryGetProperty("info", out _)).IsTrue();
        await Assert.That(root.TryGetProperty("paths", out var paths)).IsTrue();
        
        await Assert.That(paths.TryGetProperty("/tours", out _)).IsTrue();
    }
}
