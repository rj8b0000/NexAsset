using FluentValidation;

namespace NexAsset.Application.Features.Authentication.Commands.UnlockUser;

public sealed class UnlockUserCommandValidator
    : AbstractValidator<UnlockUserCommand>
{
    public UnlockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
