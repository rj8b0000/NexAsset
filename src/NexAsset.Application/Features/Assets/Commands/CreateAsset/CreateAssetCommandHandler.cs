using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Assets.Queries.GetAsset;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Assets.Commands.CreateAsset;

public sealed class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, Result<AssetResponse>>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetCategoryRepository _categoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAssetCommandHandler(
        IAssetRepository assetRepository,
        IAssetCategoryRepository categoryRepository,
        IOrganizationRepository organizationRepository,
        IBranchRepository branchRepository,
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _assetRepository = assetRepository;
        _categoryRepository = categoryRepository;
        _organizationRepository = organizationRepository;
        _branchRepository = branchRepository;
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetResponse>> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var validation = await ValidateReferencesAsync(request, cancellationToken);
        if (validation.IsFailure)
            return Result<AssetResponse>.Failure(validation.Error!);

        if (await _assetRepository.ExistsByCodeAsync(request.OrganizationId, request.AssetCode, cancellationToken))
            return Result<AssetResponse>.Failure("Asset code already exists for this organization.");

        if (!string.IsNullOrWhiteSpace(request.SerialNumber) &&
            await _assetRepository.ExistsBySerialNumberAsync(request.SerialNumber, cancellationToken))
            return Result<AssetResponse>.Failure("Asset serial number already exists.");

        var asset = AssetMapper.ToEntity(request);
        asset.AssetStatus = request.CurrentEmployeeId.HasValue ? AssetStatus.Assigned : request.AssetStatus;

        await _assetRepository.AddAsync(asset, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetResponse>.Success(AssetMapper.ToResponse(asset));
    }

    private async Task<Result> ValidateReferencesAsync(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        if (await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken) is null)
            return Result.Failure("Organization not found.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null || category.OrganizationId != request.OrganizationId)
            return Result.Failure("Asset category not found for this organization.");

        if (request.BranchId.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(request.BranchId.Value, cancellationToken);
            if (branch is null || branch.OrganizationId != request.OrganizationId)
                return Result.Failure("Branch not found for this organization.");
        }

        if (request.DepartmentId.HasValue)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId.Value, cancellationToken);
            if (department is null || department.OrganizationId != request.OrganizationId)
                return Result.Failure("Department not found for this organization.");
        }

        if (request.CurrentEmployeeId.HasValue)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.CurrentEmployeeId.Value, cancellationToken);
            if (employee is null || employee.OrganizationId != request.OrganizationId)
                return Result.Failure("Employee not found for this organization.");
        }

        return Result.Success();
    }
}
