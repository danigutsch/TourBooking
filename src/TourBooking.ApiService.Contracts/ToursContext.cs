using System.Text.Json.Serialization;
using JetBrains.Annotations;
using TourBooking.ApiService.Contracts.Models;

namespace TourBooking.ApiService.Contracts;

/// <summary>
/// Contains JSON serialization context for Tour Booking API service contracts.
/// See <see href="https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-source-generation">Source Generation</see> for more information.
/// </summary>
[JsonSerializable(typeof(GetTourDto))]
[JsonSerializable(typeof(CreateTourDto))]
[JsonSerializable(typeof(IEnumerable<GetTourDto>))]
[PublicAPI]
public partial class ToursContext : JsonSerializerContext;
