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
using NexAsset.API.Endpoints.Roles;
using NexAsset.Api.Middlewares;
using NexAsset.Application;
using NexAsset.Infrastructure;
using NexAsset.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "NexAsset.Auth";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
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
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthenticationEndpoints();
app.MapOrganizationEndpoints();
app.MapBranchEndpoints();
app.MapDepartmentEndpoints();
app.MapDesignationEndpoints();
app.MapEmployeeEndpoints();
app.MapRoleEndpoints();
app.MapPermissionEndpoints();
app.MapAssetCategoryEndpoints();
app.MapAssetEndpoints();
app.MapAssetAssignmentEndpoints();
app.MapAssetTransferEndpoints();
app.MapAssetReturnEndpoints();
app.MapEnterpriseOperationsEndpoints();
app.MapControllers();
await DatabaseInitializer.InitializeAsync(app.Services);
app.Run();
