using FluentValidation;

namespace NexAsset.Application.Features.Designations.Commands.CreateDesignation;

public sealed class CreateDesignationCommandValidator
    : AbstractValidator<CreateDesignationCommand>
{
    public CreateDesignationCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
