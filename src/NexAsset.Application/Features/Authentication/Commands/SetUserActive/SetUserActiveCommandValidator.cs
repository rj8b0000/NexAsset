using FluentValidation;

namespace NexAsset.Application.Features.Authentication.Commands.SetUserActive;

public sealed class SetUserActiveCommandValidator
    : AbstractValidator<SetUserActiveCommand>
{
    public SetUserActiveCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
