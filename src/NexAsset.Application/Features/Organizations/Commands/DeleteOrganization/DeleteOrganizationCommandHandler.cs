using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Commands.DeleteOrganization;

public sealed class DeleteOrganizationCommandHandler
    : IRequestHandler<DeleteOrganizationCommand, Result>
{
    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationCommandHandler(
        IOrganizationRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (organization is null)
        {
            return Result.Failure("Organization not found.");
        }

        // Cascade first so a deleted organization leaves no orphaned branches, employees,
        // assets, vendors, inventory, tickets, etc. behind (all soft-deleted).
        await _repository.CascadeSoftDeleteAsync(request.Id, cancellationToken);

        organization.IsDeleted = true;
        organization.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(organization);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
