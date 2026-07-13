using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetCategories.Queries.GetAssetCategory;

namespace NexAsset.Application.Features.AssetCategories.Commands.CreateAssetCategory;

public sealed class CreateAssetCategoryCommandHandler
    : IRequestHandler<CreateAssetCategoryCommand, Result<AssetCategoryResponse>>
{
    private readonly IAssetCategoryRepository _categoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAssetCategoryCommandHandler(
        IAssetCategoryRepository categoryRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetCategoryResponse>> Handle(
        CreateAssetCategoryCommand request,
        CancellationToken cancellationToken)
    {
        if (await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken) is null)
            return Result<AssetCategoryResponse>.Failure("Organization not found.");

        if (await _categoryRepository.ExistsByCodeAsync(request.OrganizationId, request.Code, cancellationToken))
            return Result<AssetCategoryResponse>.Failure("Asset category code already exists for this organization.");

        var category = AssetCategoryMapper.ToEntity(request);
        category.IsActive = true;

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AssetCategoryResponse>.Success(AssetCategoryMapper.ToResponse(category));
    }
}
