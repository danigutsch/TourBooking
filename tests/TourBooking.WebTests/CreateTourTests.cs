using System.Net.Http.Json;
using TourBooking.ApiService;
using TourBooking.Aspire.Constants;

namespace TourBooking.WebTests;

public sealed class CreateTourTests
{
    [Fact]
    public async Task CreateTourApiReturnsSuccess()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TourBooking_AppHost>(TestContext.Current.CancellationToken);

        await using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken);
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync(TestContext.Current.CancellationToken);
        using var httpClient = app.CreateHttpClient(ResourceNames.ApiService);
        await resourceNotificationService.WaitForResourceAsync(ResourceNames.ApiService, KnownResourceStates.Running, TestContext.Current.CancellationToken).WaitAsync(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        var request = new CreateTourRequest("Test Tour", "A wonderful tour", 100.0m, today, today.AddDays(5));
        var response = await httpClient.PostAsJsonAsync(new Uri("/tours", UriKind.Relative), request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}