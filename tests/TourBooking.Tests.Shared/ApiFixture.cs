extern alias api;

using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TourBooking.Tests.Shared;

[MustDisposeResource]
public sealed class ApiFixture : WebApplicationFactory<api::Program>;
