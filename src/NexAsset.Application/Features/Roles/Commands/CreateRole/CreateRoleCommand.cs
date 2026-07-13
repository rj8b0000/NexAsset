using MediatR;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(string Name)
    : IRequest<Result<RoleResponse>>;
