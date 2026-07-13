using MediatR;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Queries.GetRole;

public sealed record GetRoleQuery(Guid Id)
    : IRequest<Result<RoleResponse>>;
