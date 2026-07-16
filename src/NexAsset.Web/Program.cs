using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NexAsset.Web;
using NexAsset.Web.State;
using NexAsset.Web.Infrastructure.Services;
using NexAsset.Web.Infrastructure.Services.Mock;
using NexAsset.Web.Extensions;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// State Container registrations
builder.Services.AddSingleton<MockDatabaseService>();
builder.Services.AddSingleton<ThemeState>();
builder.Services.AddSingleton<NotificationState>();
builder.Services.AddSingleton<NavigationState>();

// Last remaining mock client: Finance — NexAsset.API has no finance/invoice endpoints yet,
// so the Finance page stays on MockDatabaseService until that backend module exists.
// Every other module now uses real NexAsset.API clients (see AddNexAssetApiInfrastructure).
builder.Services.AddScoped<IFinanceApiClient, FinanceApiClient>();

// Infrastructure preparation — API, Authentication, and cross-cutting services.
// Additive only: nothing below changes what the app does today. See the
// "Infrastructure Preparation" notes in each extension method for details.
builder.Services.AddNexAssetApiInfrastructure(builder.Configuration);
builder.Services.AddNexAssetAuthentication();
builder.Services.AddNexAssetCoreInfrastructure();

await builder.Build().RunAsync();