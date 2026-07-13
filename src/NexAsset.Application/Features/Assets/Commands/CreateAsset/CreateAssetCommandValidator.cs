using FluentValidation;

namespace NexAsset.Application.Features.Assets.Commands.CreateAsset;

public sealed class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
{
    public CreateAssetCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.AssetCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AssetName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SerialNumber).MaximumLength(100);
        RuleFor(x => x.Barcode).MaximumLength(100);
        RuleFor(x => x.QrCode).MaximumLength(500);
        RuleFor(x => x.Brand).MaximumLength(100);
        RuleFor(x => x.Model).MaximumLength(100);
        RuleFor(x => x.Vendor).MaximumLength(200);
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}
