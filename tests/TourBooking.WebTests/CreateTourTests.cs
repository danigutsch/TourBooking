using System.Net.Http.Json;
using TourBooking.ApiService.Contracts;
using TourBooking.Aspire.Constants;
using TourBooking.Tests;

namespace TourBooking.WebTests;

[Category(TestCategories.Integration)]
[NotInParallel("Aspire")]
public sealed class CreateTourTests
{
    [Test]
    public async Task CreateTourApiReturnsSuccess()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(120));
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TourBooking_AppHost>(TestContext.Current?.CancellationToken ?? cts.Token);
        await using var app = await appHost.BuildAsync(TestContext.Current?.CancellationToken ?? cts.Token);
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync(TestContext.Current?.CancellationToken ?? cts.Token);
        using var httpClient = app.CreateHttpClient(ResourceNames.ApiService);
        await resourceNotificationService.WaitForResourceAsync(ResourceNames.ApiService, KnownResourceStates.Running, TestContext.Current?.CancellationToken ?? cts.Token).WaitAsync(TimeSpan.FromSeconds(30), TestContext.Current?.CancellationToken ?? cts.Token);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        var request = new CreateTourRequest("Test Tour", "A wonderful tour", 100.0m, today, today.AddDays(5));
        var response = await httpClient.PostAsJsonAsync(new Uri("/tours", UriKind.Relative), request, TestContext.Current?.CancellationToken ?? cts.Token);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }
}
