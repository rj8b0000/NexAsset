using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.CreateDesignation;

public sealed class CreateDesignationCommandHandler
    : IRequestHandler<CreateDesignationCommand, Result<CreateDesignationResponse>>
{
    private readonly IDesignationRepository _designationRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDesignationCommandHandler(
        IDesignationRepository designationRepository,
        IDepartmentRepository departmentRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _designationRepository = designationRepository;
        _departmentRepository = departmentRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateDesignationResponse>> Handle(
        CreateDesignationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<CreateDesignationResponse>
                .Failure("Organization not found.");
        }

        if (request.DepartmentId.HasValue)
        {
            var department = await _departmentRepository.GetByIdAsync(
                request.DepartmentId.Value,
                cancellationToken);

            if (department is null ||
                department.OrganizationId != request.OrganizationId)
            {
                return Result<CreateDesignationResponse>
                    .Failure("Department not found for this organization.");
            }
        }

        var exists = await _designationRepository.ExistsByTitleAsync(
            request.OrganizationId,
            request.Title,
            cancellationToken);

        if (exists)
        {
            return Result<CreateDesignationResponse>
                .Failure("Designation title already exists for this organization.");
        }

        var designation = DesignationMapper.ToEntity(request);
        designation.IsActive = true;

        await _designationRepository.AddAsync(designation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateDesignationResponse>.Success(
            DesignationMapper.ToResponse(designation));
    }
}
