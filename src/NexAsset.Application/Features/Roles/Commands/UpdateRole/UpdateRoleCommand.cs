using MediatR;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(Guid Id, string Name)
    : IRequest<Result<RoleResponse>>;
