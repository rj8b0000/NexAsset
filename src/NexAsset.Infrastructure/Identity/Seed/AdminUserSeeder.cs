using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Identity.Seed;

public static class AdminUserSeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        const string email = "admin@nexasset.com";

        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return;

        var organization =
            await context.Organizations.FirstAsync();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            OrganizationId = organization.Id,
            IsActive = true
        };

        var result = await userManager.CreateAsync(
            user,
            "Admin@123");

        if (!result.Succeeded)
            throw new Exception("Failed to create admin user.");

        await userManager.AddToRoleAsync(
            user,
            "SuperAdmin");
    }
}