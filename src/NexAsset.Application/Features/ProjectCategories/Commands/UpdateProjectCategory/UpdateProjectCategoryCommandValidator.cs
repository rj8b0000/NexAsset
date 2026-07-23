using FluentValidation;

namespace NexAsset.Application.Features.ProjectCategories.Commands.UpdateProjectCategory;

public sealed class UpdateProjectCategoryCommandValidator : AbstractValidator<UpdateProjectCategoryCommand>
{
    public UpdateProjectCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category Name is required.")
            .MaximumLength(100).WithMessage("Category Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
