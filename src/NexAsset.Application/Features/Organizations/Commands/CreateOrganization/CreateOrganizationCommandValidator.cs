using FluentValidation;

namespace NexAsset.Application.Features.Organizations.Commands.CreateOrganization;

public sealed class CreateOrganizationCommandValidator
    : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Currency)
            .NotEmpty();

        RuleFor(x => x.TimeZone)
            .NotEmpty();
    }
}