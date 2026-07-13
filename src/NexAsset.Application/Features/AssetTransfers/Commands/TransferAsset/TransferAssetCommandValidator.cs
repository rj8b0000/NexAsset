using FluentValidation;

namespace NexAsset.Application.Features.AssetTransfers.Commands.TransferAsset;

public sealed class TransferAssetCommandValidator : AbstractValidator<TransferAssetCommand>
{
    public TransferAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.TransferDate).NotEmpty();
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}
