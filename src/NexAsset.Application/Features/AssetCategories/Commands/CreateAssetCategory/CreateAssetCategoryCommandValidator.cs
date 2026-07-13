using FluentValidation;

namespace NexAsset.Application.Features.AssetCategories.Commands.CreateAssetCategory;

public sealed class CreateAssetCategoryCommandValidator : AbstractValidator<CreateAssetCategoryCommand>
{
    public CreateAssetCategoryCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
