namespace NexAsset.Application.Features.Permissions.Queries.GetPermission;

public sealed record PermissionResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
