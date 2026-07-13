using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.AssetAssignments.Commands.AssignAsset;

public sealed class AssignAssetCommandHandler : IRequestHandler<AssignAssetCommand, Result<AssetAssignmentResponse>>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAssetAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignAssetCommandHandler(IAssetRepository assetRepository, IEmployeeRepository employeeRepository, IAssetAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _employeeRepository = employeeRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetAssignmentResponse>> Handle(AssignAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset is null)
            return Result<AssetAssignmentResponse>.Failure("Asset not found.");

        if (await _assignmentRepository.GetCurrentByAssetIdAsync(asset.Id, cancellationToken) is not null)
            return Result<AssetAssignmentResponse>.Failure("Asset is already assigned.");

        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null || employee.OrganizationId != asset.OrganizationId)
            return Result<AssetAssignmentResponse>.Failure("Employee not found for this organization.");

        var assignment = new AssetAssignment
        {
            AssetId = asset.Id,
            EmployeeId = employee.Id,
            OrganizationId = asset.OrganizationId,
            BranchId = employee.BranchId ?? asset.BranchId,
            DepartmentId = employee.DepartmentId ?? asset.DepartmentId,
            AssignedDate = request.AssignedDate,
            Status = AssetAssignmentStatus.Active,
            Remarks = request.Remarks
        };

        asset.CurrentEmployeeId = employee.Id;
        asset.BranchId = assignment.BranchId;
        asset.DepartmentId = assignment.DepartmentId;
        asset.AssetStatus = AssetStatus.Assigned;
        asset.UpdatedAtUtc = DateTime.UtcNow;

        await _assignmentRepository.AddAsync(assignment, cancellationToken);
        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetAssignmentResponse>.Success(AssetWorkflowMapper.ToAssignmentResponse(assignment));
    }
}
