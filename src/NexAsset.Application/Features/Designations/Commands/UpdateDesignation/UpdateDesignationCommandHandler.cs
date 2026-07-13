using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.UpdateDesignation;

public sealed class UpdateDesignationCommandHandler
    : IRequestHandler<UpdateDesignationCommand, Result<UpdateDesignationResponse>>
{
    private readonly IDesignationRepository _designationRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDesignationCommandHandler(
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

    public async Task<Result<UpdateDesignationResponse>> Handle(
        UpdateDesignationCommand request,
        CancellationToken cancellationToken)
    {
        var designation = await _designationRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (designation is null)
        {
            return Result<UpdateDesignationResponse>
                .Failure("Designation not found.");
        }

        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<UpdateDesignationResponse>
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
                return Result<UpdateDesignationResponse>
                    .Failure("Department not found for this organization.");
            }
        }

        var titleExists = await _designationRepository.ExistsByTitleAsync(
            request.OrganizationId,
            request.Title,
            request.Id,
            cancellationToken);

        if (titleExists)
        {
            return Result<UpdateDesignationResponse>
                .Failure("Designation title already exists for this organization.");
        }

        DesignationMapper.ApplyUpdate(request, designation);
        designation.UpdatedAtUtc = DateTime.UtcNow;

        _designationRepository.Update(designation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateDesignationResponse>.Success(
            new UpdateDesignationResponse(
                designation.Id,
                designation.Title,
                designation.IsActive));
    }
}
