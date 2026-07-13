using FluentValidation;

namespace NexAsset.Application.Features.Permissions.Commands.AssignPermissionToRole;

public sealed class AssignPermissionToRoleCommandValidator
    : AbstractValidator<AssignPermissionToRoleCommand>
{
    public AssignPermissionToRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
