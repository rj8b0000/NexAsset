using FluentValidation;

namespace NexAsset.Application.Features.AssetReturns.Commands.ReturnAsset;

public sealed class ReturnAssetCommandValidator : AbstractValidator<ReturnAssetCommand>
{
    public ReturnAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.ReturnDate).NotEmpty();
        RuleFor(x => x.InspectionNotes).MaximumLength(1000);
        RuleFor(x => x.ReturnRemarks).MaximumLength(1000);
    }
}
