using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id)
    : IRequest<Result>;
