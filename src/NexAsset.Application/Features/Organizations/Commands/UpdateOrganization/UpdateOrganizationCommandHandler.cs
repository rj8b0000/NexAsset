using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Commands.UpdateOrganization;

public sealed class UpdateOrganizationCommandHandler
    : IRequestHandler<UpdateOrganizationCommand, Result<UpdateOrganizationResponse>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrganizationCommandHandler(
        IOrganizationRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateOrganizationResponse>> Handle(
        UpdateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (organization is null)
        {
            return Result<UpdateOrganizationResponse>
                .Failure("Organization not found.");
        }

        var codeChanged = !string.Equals(
            organization.Code,
            request.Code,
            StringComparison.OrdinalIgnoreCase);

        if (codeChanged)
        {
            var codeExists = await _repository.ExistsByCodeAsync(
                request.Code,
                request.Id,
                cancellationToken);

            if (codeExists)
            {
                return Result<UpdateOrganizationResponse>
                    .Failure("Organization code already exists.");
            }
        }

        OrganizationMapper.ApplyUpdate(request, organization);
        organization.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(organization);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateOrganizationResponse>.Success(
            new UpdateOrganizationResponse(
                organization.Id,
                organization.Code,
                organization.Name,
                organization.IsActive));
    }
}
