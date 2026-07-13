using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.AssetTransfers.Commands.TransferAsset;

public sealed class TransferAssetCommandHandler : IRequestHandler<TransferAssetCommand, Result<AssetTransferResponse>>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetAssignmentRepository _assignmentRepository;
    private readonly IAssetTransferRepository _transferRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferAssetCommandHandler(IAssetRepository assetRepository, IAssetAssignmentRepository assignmentRepository, IAssetTransferRepository transferRepository, IEmployeeRepository employeeRepository, IBranchRepository branchRepository, IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _assignmentRepository = assignmentRepository;
        _transferRepository = transferRepository;
        _employeeRepository = employeeRepository;
        _branchRepository = branchRepository;
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetTransferResponse>> Handle(TransferAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset is null)
            return Result<AssetTransferResponse>.Failure("Asset not found.");

        var assignment = await _assignmentRepository.GetCurrentByAssetIdAsync(asset.Id, cancellationToken);
        if (assignment is null)
            return Result<AssetTransferResponse>.Failure("Asset is not currently assigned.");

        var validation = await ValidateTargetsAsync(asset.OrganizationId, request, cancellationToken);
        if (validation.IsFailure)
            return Result<AssetTransferResponse>.Failure(validation.Error!);

        var transfer = new AssetTransfer
        {
            AssetId = asset.Id,
            FromEmployeeId = asset.CurrentEmployeeId,
            ToEmployeeId = request.ToEmployeeId ?? asset.CurrentEmployeeId,
            FromBranchId = asset.BranchId,
            ToBranchId = request.ToBranchId ?? asset.BranchId,
            FromDepartmentId = asset.DepartmentId,
            ToDepartmentId = request.ToDepartmentId ?? asset.DepartmentId,
            TransferDate = request.TransferDate,
            IsApproved = request.IsApproved,
            Remarks = request.Remarks
        };

        asset.AssetStatus = request.IsApproved ? AssetStatus.Assigned : AssetStatus.InTransfer;

        if (request.IsApproved)
        {
            asset.CurrentEmployeeId = transfer.ToEmployeeId;
            asset.BranchId = transfer.ToBranchId;
            asset.DepartmentId = transfer.ToDepartmentId;
            assignment.EmployeeId = transfer.ToEmployeeId ?? assignment.EmployeeId;
            assignment.BranchId = transfer.ToBranchId;
            assignment.DepartmentId = transfer.ToDepartmentId;
            assignment.Status = AssetAssignmentStatus.Active;
            assignment.UpdatedAtUtc = DateTime.UtcNow;
            _assignmentRepository.Update(assignment);
        }

        asset.UpdatedAtUtc = DateTime.UtcNow;
        await _transferRepository.AddAsync(transfer, cancellationToken);
        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetTransferResponse>.Success(AssetWorkflowMapper.ToTransferResponse(transfer));
    }

    private async Task<Result> ValidateTargetsAsync(Guid organizationId, TransferAssetCommand request, CancellationToken cancellationToken)
    {
        if (request.ToEmployeeId.HasValue)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.ToEmployeeId.Value, cancellationToken);
            if (employee is null || employee.OrganizationId != organizationId)
                return Result.Failure("Employee not found for this organization.");
        }

        if (request.ToBranchId.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(request.ToBranchId.Value, cancellationToken);
            if (branch is null || branch.OrganizationId != organizationId)
                return Result.Failure("Branch not found for this organization.");
        }

        if (request.ToDepartmentId.HasValue)
        {
            var department = await _departmentRepository.GetByIdAsync(request.ToDepartmentId.Value, cancellationToken);
            if (department is null || department.OrganizationId != organizationId)
                return Result.Failure("Department not found for this organization.");
        }

        return Result.Success();
    }
}
