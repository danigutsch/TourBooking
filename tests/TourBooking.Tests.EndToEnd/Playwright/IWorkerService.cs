using JetBrains.Annotations;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public interface IWorkerService
{
    public Task ResetAsync();
    public Task DisposeAsync();
}