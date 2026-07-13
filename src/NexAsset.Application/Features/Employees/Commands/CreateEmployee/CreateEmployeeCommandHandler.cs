using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Identity;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDesignationRepository _designationRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(
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

    public async Task<Result<CreateEmployeeResponse>> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var validation = await ValidateReferencesAsync(request, cancellationToken);
        if (validation.IsFailure)
            return Result<CreateEmployeeResponse>.Failure(validation.Error!);

        if (await _employeeRepository.ExistsByCodeAsync(
                request.OrganizationId,
                request.EmployeeCode,
                cancellationToken))
        {
            return Result<CreateEmployeeResponse>
                .Failure("Employee code already exists for this organization.");
        }

        if (await _employeeRepository.ExistsByEmailAsync(
                request.Email,
                cancellationToken))
        {
            return Result<CreateEmployeeResponse>
                .Failure("Employee email already exists.");
        }

        var employee = EmployeeMapper.ToEntity(request);
        employee.IsActive = true;
        IReadOnlyCollection<string> roles = request.Roles is { Count: > 0 }
            ? request.Roles
            : new[] { "Employee" };

        var userResult = await _identityService.CreateEmployeeUserAsync(
            new CreateEmployeeUserRequest(
                employee.Id,
                employee.OrganizationId,
                employee.BranchId,
                employee.DepartmentId,
                employee.DesignationId,
                employee.Email,
                request.Password,
                roles),
            cancellationToken);

        if (userResult.IsFailure)
            return Result<CreateEmployeeResponse>.Failure(userResult.Error!);

        employee.IdentityUserId = userResult.Value;

        await _employeeRepository.AddAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateEmployeeResponse>.Success(
            EmployeeMapper.ToResponse(employee));
    }

    private async Task<Result> ValidateReferencesAsync(
        CreateEmployeeCommand request,
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
            var manager = await _employeeRepository.GetByIdAsync(
                request.ReportingManagerId.Value,
                cancellationToken);

            if (manager is null || manager.OrganizationId != request.OrganizationId)
                return Result.Failure("Reporting manager not found for this organization.");
        }

        return Result.Success();
    }
}
