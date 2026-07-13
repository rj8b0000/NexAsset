using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

namespace NexAsset.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public sealed class UpdateAssetCategoryCommandHandler
    : IRequestHandler<UpdateAssetCategoryCommand, Result<AssetCategoryResponse>>
{
    private readonly IAssetCategoryRepository _categoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAssetCategoryCommandHandler(
        IAssetCategoryRepository categoryRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetCategoryResponse>> Handle(UpdateAssetCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return Result<AssetCategoryResponse>.Failure("Asset category not found.");

        if (await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken) is null)
            return Result<AssetCategoryResponse>.Failure("Organization not found.");

        if (await _categoryRepository.ExistsByCodeAsync(request.OrganizationId, request.Code, request.Id, cancellationToken))
            return Result<AssetCategoryResponse>.Failure("Asset category code already exists for this organization.");

        AssetCategoryMapper.ApplyUpdate(request, category);
        category.UpdatedAtUtc = DateTime.UtcNow;
        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetCategoryResponse>.Success(AssetCategoryMapper.ToResponse(category));
    }
}
