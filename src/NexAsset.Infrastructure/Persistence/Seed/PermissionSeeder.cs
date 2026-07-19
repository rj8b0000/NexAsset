using Microsoft.EntityFrameworkCore;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Seed;

/// <summary>
/// Seeds the canonical permission matrix: one "Module.Action" permission per operation every
/// module actually exposes. Idempotent — inserts only codes that don't exist yet, so it is safe
/// on every startup and preserves manually created permissions.
/// </summary>
public static class PermissionSeeder
{
    // Module -> actions it supports (matching the real API surface).
    private static readonly (string Module, string[] Actions)[] Matrix =
    {
        ("Organizations", new[] { "View", "Create", "Update", "Delete" }),
        ("Branches", new[] { "View", "Create", "Update", "Delete" }),
        ("Departments", new[] { "View", "Create", "Update", "Delete" }),
        ("Designations", new[] { "View", "Create", "Update", "Delete" }),
        ("Employees", new[] { "View", "Create", "Update", "Delete", "Activate" }),
        ("Roles", new[] { "View", "Create", "Update", "Delete", "AssignPermissions" }),
        ("Users", new[] { "View", "Create", "Update", "ManageRoles", "ResetPassword" }),
        ("Permissions", new[] { "View", "Create", "Update", "Delete" }),
        ("AssetCategories", new[] { "View", "Create", "Update", "Delete" }),
        ("Assets", new[] { "View", "Create", "Update", "Delete" }),
        ("AssetAssignments", new[] { "Assign", "Unassign", "Transfer", "Return", "ViewHistory" }),
        ("Vendors", new[] { "View", "Create", "Update", "Delete" }),
        ("PurchaseRequests", new[] { "View", "Create", "Approve" }),
        ("PurchaseOrders", new[] { "View", "Create", "UpdateStatus" }),
        ("Inventory", new[] { "View", "Create", "Update", "RecordMovement" }),
        ("Consumables", new[] { "View", "Create", "Update" }),
        ("Maintenance", new[] { "View", "Create", "UpdateStatus" }),
        ("Customers", new[] { "View", "Create", "Update", "Delete" }),
        ("ServiceTickets", new[] { "View", "Create", "Update" }),
        ("Notifications", new[] { "View", "Create", "MarkRead" }),
        ("AuditLogs", new[] { "View", "Create" }),
        ("SystemSettings", new[] { "View", "Manage" }),
        ("Dashboard", new[] { "View" }),
        ("Reports", new[] { "View" }),
        ("Finance", new[] { "View" }),
    };

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var existing = await context.Permissions
            .Select(p => p.Code)
            .ToListAsync();
        var existingSet = existing.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var added = false;
        foreach (var (module, actions) in Matrix)
        {
            foreach (var action in actions)
            {
                var code = $"{module}.{action}";
                if (existingSet.Contains(code))
                    continue;

                context.Permissions.Add(new Permission
                {
                    Code = code,
                    Name = $"{Humanize(action)} {Humanize(module)}",
                    Description = $"Allows the '{Humanize(action)}' operation in the {Humanize(module)} module.",
                    IsActive = true
                });
                added = true;
            }
        }

        if (added)
            await context.SaveChangesAsync();
    }

    private static string Humanize(string pascal)
    {
        var chars = new List<char>();
        for (var i = 0; i < pascal.Length; i++)
        {
            if (i > 0 && char.IsUpper(pascal[i]) && !char.IsUpper(pascal[i - 1]))
                chars.Add(' ');
            chars.Add(pascal[i]);
        }
        return new string(chars.ToArray());
    }
}
