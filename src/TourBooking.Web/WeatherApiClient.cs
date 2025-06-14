using JetBrains.Annotations;

namespace TourBooking.Web;

/// <summary>
/// Client for retrieving weather forecasts from the weather API.
/// </summary>
[UsedImplicitly]
internal sealed class WeatherApiClient(HttpClient httpClient)
{
    /// <summary>
    /// Retrieves weather forecasts asynchronously from the API.
    /// </summary>
    /// <param name="maxItems">The maximum number of forecasts to return.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An array of <see cref="WeatherForecast"/> objects.</returns>
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}