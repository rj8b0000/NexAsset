using NexAsset.Application.Features.Permissions.Commands.CreatePermission;
using NexAsset.Application.Features.Permissions.Commands.UpdatePermission;
using NexAsset.Application.Features.Permissions.Queries.GetPermission;
using NexAsset.Application.Features.Permissions.Queries.GetPermissions;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class PermissionMapper
{
    [MapperIgnoreTarget(nameof(Permission.Id))]
    [MapperIgnoreTarget(nameof(Permission.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Permission.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Permission.IsDeleted))]
    [MapperIgnoreTarget(nameof(Permission.DeletedAtUtc))]
    public static partial Permission ToEntity(CreatePermissionCommand command);

    public static partial PermissionResponse ToResponse(Permission permission);

    public static partial PermissionListItemResponse ToListItemResponse(Permission permission);

    [MapperIgnoreTarget(nameof(Permission.Id))]
    [MapperIgnoreTarget(nameof(Permission.CreatedAtUtc))]
    [MapperIgnoreTarget(nameof(Permission.UpdatedAtUtc))]
    [MapperIgnoreTarget(nameof(Permission.IsDeleted))]
    [MapperIgnoreTarget(nameof(Permission.DeletedAtUtc))]
    [MapperIgnoreSource(nameof(UpdatePermissionCommand.Id))]
    public static partial void ApplyUpdate(
        UpdatePermissionCommand command,
        Permission permission);
}
