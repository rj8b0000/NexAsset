using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectCategories.Commands.CreateProjectCategory;

public sealed class CreateProjectCategoryCommandHandler : IRequestHandler<CreateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    private readonly IProjectCategoryRepository _repository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCategoryCommandHandler(
        IProjectCategoryRepository repository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectCategoryResponse>> Handle(CreateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var categoryName = request.Name.Trim();
            var orgId = request.OrganizationId;
            if (orgId == Guid.Empty)
            {
                var orgs = await _organizationRepository.GetAllAsync(cancellationToken);
                if (orgs != null && orgs.Count > 0)
                {
                    orgId = orgs[0].Id;
                }
                else
                {
                    return Result<ProjectCategoryResponse>.Failure("No valid organization found to attach project category.");
                }
            }

            var exists = await _repository.ExistsByNameAsync(orgId, categoryName, cancellationToken);
            if (exists)
            {
                return Result<ProjectCategoryResponse>.Failure($"Project category name '{categoryName}' already exists.");
            }

            var cmd = request with { OrganizationId = orgId, Name = categoryName };
            var category = ProjectCategoryMapper.ToEntity(cmd);
            category.OrganizationId = orgId;
            category.Name = categoryName;

            await _repository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ProjectCategoryResponse>.Success(ProjectCategoryMapper.ToResponse(category));
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
                ex.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Result<ProjectCategoryResponse>.Failure($"Project category name '{request.Name.Trim()}' already exists in this organization.");
            }
            return Result<ProjectCategoryResponse>.Failure($"Error creating project category: {ex.Message}");
        }
    }
}
