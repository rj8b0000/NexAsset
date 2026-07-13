using FluentValidation;

namespace NexAsset.Application.Features.Permissions.Commands.RemovePermissionFromRole;

public sealed class RemovePermissionFromRoleCommandValidator
    : AbstractValidator<RemovePermissionFromRoleCommand>
{
    public RemovePermissionFromRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
