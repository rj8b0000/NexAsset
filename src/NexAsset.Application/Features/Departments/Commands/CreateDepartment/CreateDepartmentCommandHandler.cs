using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Result<CreateDepartmentResponse>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDepartmentCommandHandler(
        IDepartmentRepository departmentRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateDepartmentResponse>> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<CreateDepartmentResponse>
                .Failure("Organization not found.");
        }

        var exists = await _departmentRepository.ExistsByCodeAsync(
            request.OrganizationId,
            request.Code,
            cancellationToken);

        if (exists)
        {
            return Result<CreateDepartmentResponse>
                .Failure("Department code already exists for this organization.");
        }

        var department = DepartmentMapper.ToEntity(request);
        department.IsActive = true;

        await _departmentRepository.AddAsync(department, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateDepartmentResponse>.Success(
            DepartmentMapper.ToResponse(department));
    }
}
