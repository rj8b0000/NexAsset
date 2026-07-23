using FluentValidation;

namespace NexAsset.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId is required.");

        RuleFor(x => x.ProjectCode)
            .NotEmpty().WithMessage("Project Code is required.")
            .MaximumLength(50).WithMessage("Project Code must not exceed 50 characters.");

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("Project Name is required.")
            .MaximumLength(200).WithMessage("Project Name must not exceed 200 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.");

        RuleFor(x => x.InternalRemarks)
            .MaximumLength(2000).WithMessage("Internal Remarks must not exceed 2000 characters.");

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate)
            .WithMessage("Start Date cannot be later than End Date.");
    }
}
