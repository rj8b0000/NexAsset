using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Creates a standalone login account (no employee record). Such accounts draw their
/// permissions from <paramref name="Roles"/> — this is how an administrator account is made.
/// <paramref name="OrganizationId"/> is the organization the account may see; leave it null
/// only for system-wide accounts (a SuperAdmin), which are not restricted to one organization.
/// </summary>
public sealed record CreateUserCommand(
    string Email,
    string Password,
    Guid? OrganizationId,
    IReadOnlyCollection<string> Roles)
    : IRequest<Result<Guid>>;
