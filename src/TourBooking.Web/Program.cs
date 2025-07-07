using TourBooking.ApiService.Contracts;
using TourBooking.Constants;
using TourBooking.Core.Infrastructure;
using TourBooking.ServiceDefaults;
using TourBooking.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddCoreInfrastructureServices();

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<ToursApiClient>(client => client.BaseAddress = new Uri($"https+http://{ResourceNames.ApiService}"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

await app.RunAsync();
