using Microsoft.EntityFrameworkCore;
using NexAsset.Domain.Entities;

namespace NexAsset.Infrastructure.Persistence.Seed;

public static class ProjectCategorySeeder
{
    private static readonly string[] Suggestions =
    [
        "Solar EPC",
        "Electrical Substation",
        "Transmission Line",
        "Distribution Line",
        "Underground Cabling",
        "HT/LT Panel Installation",
        "Operations & Maintenance",
        "Electrical Testing",
        "Industrial Automation",
        "Civil Construction",
        "Mechanical EPC",
        "Infrastructure",
        "Water Treatment",
        "Facility Management",
        "Residential Solar",
        "Commercial Solar"
    ];

    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        if (!await ProjectCategoriesTableExistsAsync(context, cancellationToken))
            return;

        var organizations = await context.Organizations
            .AsNoTracking()
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var organizationId in organizations)
        {
            var existing = await context.ProjectCategories
                .Where(x => x.OrganizationId == organizationId)
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);
            var existingSet = existing.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var suggestion in Suggestions)
            {
                if (existingSet.Contains(suggestion))
                    continue;

                context.ProjectCategories.Add(new ProjectCategory
                {
                    OrganizationId = organizationId,
                    Name = suggestion,
                    Description = $"{suggestion} project workspace category.",
                    IsSystemSuggested = true,
                    IsActive = true
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<bool> ProjectCategoriesTableExistsAsync(
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var exists = await context.Database
            .SqlQueryRaw<bool>(
                """
                SELECT EXISTS (
                    SELECT 1
                    FROM information_schema.tables
                    WHERE table_schema = 'public'
                      AND table_name = 'ProjectCategories'
                ) AS "Value"
                """)
            .SingleAsync(cancellationToken);

        return exists;
    }
}
