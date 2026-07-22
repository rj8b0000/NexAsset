using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Infrastructure.Identity;
using NexAsset.Infrastructure.Identity.Services;
using NexAsset.Infrastructure.Persistence;
using NexAsset.Infrastructure.Persistence.Repositories;
using NexAsset.Infrastructure.Services;

namespace NexAsset.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDesignationRepository, DesignationRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssetAssignmentRepository, AssetAssignmentRepository>();
        services.AddScoped<IAssetTransferRepository, AssetTransferRepository>();
        services.AddScoped<IAssetReturnRepository, AssetReturnRepository>();
        services.AddScoped<IEnterpriseOperationsRepository, EnterpriseOperationsRepository>();
        services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IDraftSessionRepository, DraftSessionRepository>();
        services.AddScoped<IProjectTeamRepository, ProjectTeamRepository>();
        services.AddScoped<IProjectAssetRepository, ProjectAssetRepository>();
        services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
        services.AddScoped<IProjectParameterRepository, ProjectParameterRepository>();
        services.AddScoped<IProjectBudgetRepository, ProjectBudgetRepository>();
        services.AddScoped<IProjectRiskRepository, ProjectRiskRepository>();
        services.AddScoped<IProjectTimelineRepository, ProjectTimelineRepository>();
        services.AddScoped<IProjectActivityRepository, ProjectActivityRepository>();
        services.AddScoped<ISavedFilterRepository, SavedFilterRepository>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Permission enforcement: cached role+designation permission resolution.
        services.AddMemoryCache();
        services.AddScoped<Authorization.IEffectivePermissionService, Authorization.EffectivePermissionService>();

        // Organization boundary applied to every query by ApplicationDbContext.
        services.AddScoped<ITenantContext, Authorization.TenantContext>();


        return services;
    }
}
