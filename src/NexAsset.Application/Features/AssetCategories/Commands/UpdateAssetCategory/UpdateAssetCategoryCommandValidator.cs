using FluentValidation;

namespace NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public sealed class UpdateAssetCategoryCommandValidator : AbstractValidator<UpdateAssetCategoryCommand>
{
    public UpdateAssetCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
