using FluentValidation;

namespace NexAsset.Application.Features.Roles.Commands.AssignRole;

public sealed class AssignRoleCommandValidator
    : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .MaximumLength(256);
    }
}
