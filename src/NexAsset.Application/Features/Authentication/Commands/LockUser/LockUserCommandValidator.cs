using FluentValidation;

namespace NexAsset.Application.Features.Authentication.Commands.LockUser;

public sealed class LockUserCommandValidator
    : AbstractValidator<LockUserCommand>
{
    public LockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
