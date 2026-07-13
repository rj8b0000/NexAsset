using FluentValidation;

namespace NexAsset.Application.Features.AssetAssignments.Commands.UnassignAsset;

public sealed class UnassignAssetCommandValidator : AbstractValidator<UnassignAssetCommand>
{
    public UnassignAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.UnassignedDate).NotEmpty();
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}
