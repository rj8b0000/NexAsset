using NexAsset.API.Endpoints.Authentication;
using NexAsset.API.Endpoints.AssetAssignments;
using NexAsset.API.Endpoints.AssetCategories;
using NexAsset.API.Endpoints.AssetReturns;
using NexAsset.API.Endpoints.Assets;
using NexAsset.API.Endpoints.AssetTransfers;
using NexAsset.API.Endpoints.Branches;
using NexAsset.API.Endpoints.Departments;
using NexAsset.API.Endpoints.Designations;
using NexAsset.API.Endpoints.Employees;
using NexAsset.API.Endpoints.EnterpriseOperations;
using NexAsset.API.Endpoints.Organizations;
using NexAsset.API.Endpoints.Permissions;
using NexAsset.API.Endpoints.ProjectCategories;
using NexAsset.API.Endpoints.Projects;
using NexAsset.API.Endpoints.Roles;
using NexAsset.API.Endpoints.Users;
using NexAsset.Api.Middlewares;
using NexAsset.Application;
using NexAsset.Infrastructure;
using NexAsset.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

// CORS for the Blazor WASM frontend. Credentials must be allowed so the browser will
// send/receive the ASP.NET Identity auth cookie on cross-origin (different-port) calls;
// AllowCredentials() forbids a wildcard origin, hence the explicit dev origins.
const string WasmCorsPolicy = "WasmClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(WasmCorsPolicy, policy => policy
        .WithOrigins("http://localhost:5174", "https://localhost:7225")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "NexAsset.Auth";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);

    // This host only serves the API — return status codes instead of the cookie
    // middleware's default browser redirects so the WASM client sees 401/403.
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(WasmCorsPolicy);
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
// Must sit after authentication: it reads the signed-in user to decide which organization's
// data the request may touch, which the DbContext query filters then enforce.
app.UseMiddleware<TenantResolutionMiddleware>();
app.MapAuthenticationEndpoints();
app.MapOrganizationEndpoints();
app.MapBranchEndpoints();
app.MapDepartmentEndpoints();
app.MapDesignationEndpoints();
app.MapEmployeeEndpoints();
app.MapRoleEndpoints();
app.MapUserEndpoints();
app.MapPermissionEndpoints();
app.MapAssetCategoryEndpoints();
app.MapAssetEndpoints();
app.MapAssetAssignmentEndpoints();
app.MapAssetTransferEndpoints();
app.MapAssetReturnEndpoints();
app.MapEnterpriseOperationsEndpoints();
app.MapProjectCategoryEndpoints();
app.MapProjectWorkspaceEndpoints();
app.MapControllers();
await DatabaseInitializer.InitializeAsync(app.Services);
app.Run();
