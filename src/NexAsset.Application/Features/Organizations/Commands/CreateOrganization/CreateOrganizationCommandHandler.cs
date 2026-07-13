using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.Organizations.Commands.CreateOrganization;

public sealed class CreateOrganizationCommandHandler
    : IRequestHandler<CreateOrganizationCommand, Result<CreateOrganizationResponse>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrganizationCommandHandler(
        IOrganizationRepository repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateOrganizationResponse>> Handle(
        CreateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsByCodeAsync(
            request.Code,
            cancellationToken);

        if (exists)
        {
            return Result<CreateOrganizationResponse>
                .Failure("Organization code already exists.");
        }

        var organization = OrganizationMapper.ToEntity(request);
        organization.IsActive = true;

        await _repository.AddAsync(
            organization,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateOrganizationResponse>.Success(
            OrganizationMapper.ToResponse(organization));
    }
}