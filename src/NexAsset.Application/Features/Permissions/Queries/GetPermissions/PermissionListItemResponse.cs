namespace NexAsset.Application.Features.Permissions.Queries.GetPermissions;

public sealed record PermissionListItemResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive);
