extern alias web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TourBooking.Tests.Shared;

[MustDisposeResource]
public class WebFixture : WebApplicationFactory<web::Program>
{
    public WebFixture()
    {
        UseKestrel(0);
    }
}
