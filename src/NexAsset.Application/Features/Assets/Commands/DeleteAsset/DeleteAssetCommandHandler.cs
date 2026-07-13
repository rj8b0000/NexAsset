using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Assets.Commands.DeleteAsset;

public sealed class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, Result>
{
    private readonly IAssetRepository _repository;
    private readonly IAssetAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAssetCommandHandler(IAssetRepository repository, IAssetAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset is null)
            return Result.Failure("Asset not found.");

        if (await _assignmentRepository.GetCurrentByAssetIdAsync(asset.Id, cancellationToken) is not null)
            return Result.Failure("Assigned asset cannot be deleted.");

        asset.IsDeleted = true;
        asset.AssetStatus = AssetStatus.Retired;
        asset.DeletedAtUtc = DateTime.UtcNow;
        _repository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
