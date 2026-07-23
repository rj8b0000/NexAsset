using FluentValidation;

namespace NexAsset.Application.Features.ProjectCategories.Commands.CreateProjectCategory;

public sealed class CreateProjectCategoryCommandValidator : AbstractValidator<CreateProjectCategoryCommand>
{
    public CreateProjectCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category Name is required.")
            .MaximumLength(100).WithMessage("Category Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
