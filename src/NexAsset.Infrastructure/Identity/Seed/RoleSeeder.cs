using Microsoft.AspNetCore.Identity;

namespace NexAsset.Infrastructure.Identity.Seed;

public class RoleSeeder
{
    public static async Task SeedAsync(
        RoleManager<ApplicationRole> roleManager)
    {
        string[] roles =
        {
            "SuperAdmin",
            "OrganizationAdmin",
            "Manager",
            "Employee"
        };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            await roleManager.CreateAsync(
                new ApplicationRole
                {
                    Name = role
                });
        }
    }
}