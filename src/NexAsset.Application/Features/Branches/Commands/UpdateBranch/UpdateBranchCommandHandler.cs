using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.UpdateBranch;

public sealed class UpdateBranchCommandHandler
    : IRequestHandler<UpdateBranchCommand, Result<UpdateBranchResponse>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBranchCommandHandler(
        IBranchRepository branchRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateBranchResponse>> Handle(
        UpdateBranchCommand request,
        CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (branch is null)
        {
            return Result<UpdateBranchResponse>
                .Failure("Branch not found.");
        }

        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<UpdateBranchResponse>
                .Failure("Organization not found.");
        }

        var codeExists = await _branchRepository.ExistsByCodeAsync(
            request.OrganizationId,
            request.Code,
            request.Id,
            cancellationToken);

        if (codeExists)
        {
            return Result<UpdateBranchResponse>
                .Failure("Branch code already exists for this organization.");
        }

        BranchMapper.ApplyUpdate(request, branch);
        branch.UpdatedAtUtc = DateTime.UtcNow;

        _branchRepository.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateBranchResponse>.Success(
            new UpdateBranchResponse(
                branch.Id,
                branch.Code,
                branch.Name,
                branch.IsActive));
    }
}
