using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDesignationRepository _designationRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IOrganizationRepository organizationRepository,
        IBranchRepository branchRepository,
        IDepartmentRepository departmentRepository,
        IDesignationRepository designationRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _organizationRepository = organizationRepository;
        _branchRepository = branchRepository;
        _departmentRepository = departmentRepository;
        _designationRepository = designationRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateEmployeeResponse>> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)
            return Result<UpdateEmployeeResponse>.Failure("Employee not found.");

        var validation = await ValidateReferencesAsync(request, cancellationToken);
        if (validation.IsFailure)
            return Result<UpdateEmployeeResponse>.Failure(validation.Error!);

        if (await _employeeRepository.ExistsByCodeAsync(
                request.OrganizationId,
                request.EmployeeCode,
                request.Id,
                cancellationToken))
        {
            return Result<UpdateEmployeeResponse>
                .Failure("Employee code already exists for this organization.");
        }

        if (await _employeeRepository.ExistsByEmailAsync(
                request.Email,
                request.Id,
                cancellationToken))
        {
            return Result<UpdateEmployeeResponse>
                .Failure("Employee email already exists.");
        }

        EmployeeMapper.ApplyUpdate(request, employee);
        employee.UpdatedAtUtc = DateTime.UtcNow;

        if (employee.IdentityUserId.HasValue)
        {
            var identityResult = await _identityService.UpdateEmployeeUserAsync(
                employee.IdentityUserId.Value,
                employee.OrganizationId,
                employee.BranchId,
                employee.DepartmentId,
                employee.DesignationId,
                employee.Email,
                employee.IsActive,
                cancellationToken);

            if (identityResult.IsFailure)
                return Result<UpdateEmployeeResponse>.Failure(identityResult.Error!);
        }

        _employeeRepository.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateEmployeeResponse>.Success(
            new UpdateEmployeeResponse(
                employee.Id,
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.IsActive));
    }

    private async Task<Result> ValidateReferencesAsync(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
            return Result.Failure("Organization not found.");

        if (request.BranchId.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(
                request.BranchId.Value,
                cancellationToken);

            if (branch is null || branch.OrganizationId != request.OrganizationId)
                return Result.Failure("Branch not found for this organization.");
        }

        if (request.DepartmentId.HasValue)
        {
            var department = await _departmentRepository.GetByIdAsync(
                request.DepartmentId.Value,
                cancellationToken);

            if (department is null || department.OrganizationId != request.OrganizationId)
                return Result.Failure("Department not found for this organization.");
        }

        if (request.DesignationId.HasValue)
        {
            var designation = await _designationRepository.GetByIdAsync(
                request.DesignationId.Value,
                cancellationToken);

            if (designation is null || designation.OrganizationId != request.OrganizationId)
                return Result.Failure("Designation not found for this organization.");
        }

        if (request.ReportingManagerId.HasValue)
        {
            if (request.ReportingManagerId.Value == request.Id)
                return Result.Failure("Employee cannot report to themselves.");

            var manager = await _employeeRepository.GetByIdAsync(
                request.ReportingManagerId.Value,
                cancellationToken);

            if (manager is null || manager.OrganizationId != request.OrganizationId)
                return Result.Failure("Reporting manager not found for this organization.");
        }

        return Result.Success();
    }
}
