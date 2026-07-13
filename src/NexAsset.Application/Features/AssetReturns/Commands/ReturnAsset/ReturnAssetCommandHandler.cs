using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.AssetReturns.Commands.ReturnAsset;

public sealed class ReturnAssetCommandHandler : IRequestHandler<ReturnAssetCommand, Result<AssetReturnResponse>>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetAssignmentRepository _assignmentRepository;
    private readonly IAssetReturnRepository _returnRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturnAssetCommandHandler(IAssetRepository assetRepository, IAssetAssignmentRepository assignmentRepository, IAssetReturnRepository returnRepository, IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _assignmentRepository = assignmentRepository;
        _returnRepository = returnRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetReturnResponse>> Handle(ReturnAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset is null)
            return Result<AssetReturnResponse>.Failure("Asset not found.");

        var assignment = await _assignmentRepository.GetCurrentByAssetIdAsync(asset.Id, cancellationToken);
        if (assignment is null)
            return Result<AssetReturnResponse>.Failure("Asset is not currently assigned.");

        var assetReturn = new AssetReturn
        {
            AssetId = asset.Id,
            EmployeeId = assignment.EmployeeId,
            ReturnDate = request.ReturnDate,
            InspectionNotes = request.InspectionNotes,
            ReturnRemarks = request.ReturnRemarks,
            IsAssetUsable = request.IsAssetUsable
        };

        assignment.Status = AssetAssignmentStatus.Returned;
        assignment.UnassignedDate = request.ReturnDate;
        assignment.UpdatedAtUtc = DateTime.UtcNow;

        asset.CurrentEmployeeId = null;
        asset.AssetStatus = request.IsAssetUsable ? AssetStatus.Available : AssetStatus.Damaged;
        asset.Remarks = request.ReturnRemarks ?? asset.Remarks;
        asset.UpdatedAtUtc = DateTime.UtcNow;

        await _returnRepository.AddAsync(assetReturn, cancellationToken);
        _assignmentRepository.Update(assignment);
        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetReturnResponse>.Success(AssetWorkflowMapper.ToReturnResponse(assetReturn));
    }
}
