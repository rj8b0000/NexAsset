using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Permissions.Queries.GetPermission;

public sealed record GetPermissionQuery(Guid Id)
    : IRequest<Result<PermissionResponse>>;
