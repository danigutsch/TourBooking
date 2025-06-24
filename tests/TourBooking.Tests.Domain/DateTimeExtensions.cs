namespace TourBooking.Tests.Domain;

internal static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);
}
