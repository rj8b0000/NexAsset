namespace NexAsset.API.Authorization;

/// <summary>
/// Maps an incoming request to the "Module.Action" permission code it requires, derived from the
/// route pattern and HTTP method. Returning null means the route needs authentication only.
/// The codes produced here must exist in <c>PermissionSeeder</c>'s canonical matrix.
/// </summary>
public static class PermissionRouteConvention
{
    private static readonly Dictionary<string, string> Modules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["organizations"] = "Organizations",
        ["branches"] = "Branches",
        ["departments"] = "Departments",
        ["designations"] = "Designations",
        ["employees"] = "Employees",
        ["roles"] = "Roles",
        ["permissions"] = "Permissions",
        ["asset-categories"] = "AssetCategories",
        ["assets"] = "Assets",
        ["asset-assignments"] = "AssetAssignments",
        ["asset-transfers"] = "AssetAssignments",
        ["asset-returns"] = "AssetAssignments",
        ["vendors"] = "Vendors",
        ["customers"] = "Customers",
        ["purchase-requests"] = "PurchaseRequests",
        ["purchase-orders"] = "PurchaseOrders",
        ["inventory"] = "Inventory",
        ["consumables"] = "Consumables",
        ["maintenance"] = "Maintenance",
        ["service-tickets"] = "ServiceTickets",
        ["notifications"] = "Notifications",
        ["audit-logs"] = "AuditLogs",
        ["system-settings"] = "SystemSettings",
        ["dashboard"] = "Dashboard",
    };

    public static string? Resolve(HttpContext http)
    {
        if (http.GetEndpoint() is not RouteEndpoint route || route.RoutePattern.RawText is not { } pattern)
            return null;

        // Normalize "/api/enterprise-operations/vendors/{id:guid}" -> ["vendors", "*"]
        var segments = pattern.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.StartsWith('{') ? "*" : s.ToLowerInvariant())
            .ToArray();
        var start = 0;
        if (start < segments.Length && segments[start] == "api") start++;
        if (start < segments.Length && segments[start] == "enterprise-operations") start++;
        if (start >= segments.Length)
            return null;

        var moduleSegment = segments[start];
        var tail = segments[(start + 1)..];
        if (!Modules.TryGetValue(moduleSegment, out var module))
            return null;

        var method = http.Request.Method.ToUpperInvariant();
        var isPost = method == "POST";

        switch (moduleSegment)
        {
            case "asset-assignments":
                return isPost
                    ? (tail is ["assign"] ? "AssetAssignments.Assign" : "AssetAssignments.Unassign")
                    : "AssetAssignments.ViewHistory";
            case "asset-transfers":
                return isPost ? "AssetAssignments.Transfer" : "AssetAssignments.ViewHistory";
            case "asset-returns":
                return isPost ? "AssetAssignments.Return" : "AssetAssignments.ViewHistory";
            case "permissions" when tail.Length > 0 && tail[0] == "roles":
                return method == "GET" ? "Roles.View" : "Roles.AssignPermissions";
            case "permissions" when tail.Length > 0 && tail[0] == "designations":
                return method == "GET" ? "Designations.View" : "Designations.Update";
            case "roles" when tail is ["assign"]:
                return "Roles.Update";
            case "employees" when tail.Length > 0 && tail[^1] == "activate":
                return "Employees.Activate";
            case "purchase-requests" when tail.Length > 0 && tail[^1] == "status":
                return "PurchaseRequests.Approve";
            case "purchase-orders" when tail.Length > 0 && tail[^1] == "status":
                return "PurchaseOrders.UpdateStatus";
            case "maintenance" when tail.Length > 0 && tail[^1] == "status":
                return "Maintenance.UpdateStatus";
            case "inventory" when tail is ["stock-movements"]:
                return "Inventory.RecordMovement";
            case "inventory" when tail.Length > 0 && tail[^1] == "stock-history":
                return "Inventory.View";
            case "notifications" when tail.Length > 0 && tail[^1] == "read":
                return "Notifications.MarkRead";
            case "system-settings":
                return method == "GET" ? "SystemSettings.View" : "SystemSettings.Manage";
            case "dashboard":
                return "Dashboard.View";
        }

        return method switch
        {
            "GET" => $"{module}.View",
            "POST" => $"{module}.Create",
            "PUT" or "PATCH" => $"{module}.Update",
            "DELETE" => $"{module}.Delete",
            _ => null,
        };
    }
}
