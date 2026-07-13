using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Infrastructure.Identity;
using NexAsset.Infrastructure.Identity.Services;
using NexAsset.Infrastructure.Persistence;
using NexAsset.Infrastructure.Persistence.Repositories;

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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        

        return services;
    }
}
