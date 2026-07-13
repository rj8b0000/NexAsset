using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.AssetAssignments.Commands.UnassignAsset;

public sealed class UnassignAssetCommandHandler : IRequestHandler<UnassignAssetCommand, Result>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnassignAssetCommandHandler(IAssetRepository assetRepository, IAssetAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnassignAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset is null)
            return Result.Failure("Asset not found.");

        var assignment = await _assignmentRepository.GetCurrentByAssetIdAsync(asset.Id, cancellationToken);
        if (assignment is null)
            return Result.Failure("Asset is not currently assigned.");

        assignment.Status = AssetAssignmentStatus.Unassigned;
        assignment.UnassignedDate = request.UnassignedDate;
        assignment.Remarks = request.Remarks ?? assignment.Remarks;
        assignment.UpdatedAtUtc = DateTime.UtcNow;

        asset.CurrentEmployeeId = null;
        asset.AssetStatus = AssetStatus.Available;
        asset.UpdatedAtUtc = DateTime.UtcNow;

        _assignmentRepository.Update(assignment);
        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
