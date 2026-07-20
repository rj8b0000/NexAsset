using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NexAsset.Infrastructure.Identity;
using NexAsset.Infrastructure.Identity.Seed;

namespace NexAsset.Infrastructure.Persistence.Seed;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        await OrganizationSeeder.SeedAsync(context);

        await PermissionSeeder.SeedAsync(context);

        await ProjectCategorySeeder.SeedAsync(context);

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await RoleSeeder.SeedAsync(roleManager);

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await AdminUserSeeder.SeedAsync(
            userManager,
            context);
    }
}
