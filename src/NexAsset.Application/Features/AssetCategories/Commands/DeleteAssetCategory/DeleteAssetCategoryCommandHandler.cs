using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetCategories.Commands.DeleteAssetCategory;

public sealed class DeleteAssetCategoryCommandHandler : IRequestHandler<DeleteAssetCategoryCommand, Result>
{
    private readonly IAssetCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAssetCategoryCommandHandler(IAssetCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAssetCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return Result.Failure("Asset category not found.");

        category.IsDeleted = true;
        category.IsActive = false;
        category.DeletedAtUtc = DateTime.UtcNow;
        _repository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
