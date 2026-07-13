namespace NexAsset.Application.Features.Permissions.Commands.UpdatePermission;

public sealed record UpdatePermissionRequest(
    string Code,
    string Name,
    string? Description,
    bool IsActive);
