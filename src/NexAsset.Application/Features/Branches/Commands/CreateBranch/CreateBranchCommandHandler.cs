using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandHandler
    : IRequestHandler<CreateBranchCommand, Result<CreateBranchResponse>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBranchCommandHandler(
        IBranchRepository branchRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateBranchResponse>> Handle(
        CreateBranchCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(
            request.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result<CreateBranchResponse>
                .Failure("Organization not found.");
        }

        var exists = await _branchRepository.ExistsByCodeAsync(
            request.OrganizationId,
            request.Code,
            cancellationToken);

        if (exists)
        {
            return Result<CreateBranchResponse>
                .Failure("Branch code already exists for this organization.");
        }

        var branch = BranchMapper.ToEntity(request);
        branch.IsActive = true;

        await _branchRepository.AddAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateBranchResponse>.Success(
            BranchMapper.ToResponse(branch));
    }
}
