using JetBrains.Annotations;

namespace TourBooking.Web;

/// <summary>
/// Represents a weather forecast.
/// </summary>
[PublicAPI]
internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
