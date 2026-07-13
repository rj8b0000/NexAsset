using FluentValidation;

namespace NexAsset.Application.Features.AssetAssignments.Commands.AssignAsset;

public sealed class AssignAssetCommandValidator : AbstractValidator<AssignAssetCommand>
{
    public AssignAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.AssignedDate).NotEmpty();
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}
