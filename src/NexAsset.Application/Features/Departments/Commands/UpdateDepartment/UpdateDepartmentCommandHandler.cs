using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, Result<UpdateDepartmentResponse>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDepartmentCommandHandler(
        IDepartmentRepository departmentRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateDepartmentResponse>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
        {
            return Result<UpdateDepartmentResponse>
                .Failure("Department not found.");
        }

        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<UpdateDepartmentResponse>
                .Failure("Organization not found.");
        }

        var codeExists = await _departmentRepository.ExistsByCodeAsync(
            request.OrganizationId,
            request.Code,
            request.Id,
            cancellationToken);

        if (codeExists)
        {
            return Result<UpdateDepartmentResponse>
                .Failure("Department code already exists for this organization.");
        }

        DepartmentMapper.ApplyUpdate(request, department);
        department.UpdatedAtUtc = DateTime.UtcNow;

        _departmentRepository.Update(department);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateDepartmentResponse>.Success(
            new UpdateDepartmentResponse(
                department.Id,
                department.Code,
                department.Name,
                department.IsActive));
    }
}
