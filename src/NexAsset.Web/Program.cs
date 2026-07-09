using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NexAsset.Web;
using NexAsset.Web.State;
using NexAsset.Web.Infrastructure.Services;
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
builder.Services.AddSingleton<AuthenticationState>();

// API Client registrations
builder.Services.AddScoped<IAssetApiClient, AssetApiClient>();
builder.Services.AddScoped<IEmployeeApiClient, EmployeeApiClient>();
builder.Services.AddScoped<IOrganizationApiClient, OrganizationApiClient>();
builder.Services.AddScoped<IProcurementApiClient, ProcurementApiClient>();
builder.Services.AddScoped<IMaintenanceApiClient, MaintenanceApiClient>();
builder.Services.AddScoped<IFinanceApiClient, FinanceApiClient>();
builder.Services.AddScoped<IAuditLogApiClient, AuditLogApiClient>();

await builder.Build().RunAsync();