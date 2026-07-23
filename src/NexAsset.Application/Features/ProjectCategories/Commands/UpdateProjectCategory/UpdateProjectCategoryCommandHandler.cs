using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Commands.UpdateProjectCategory;

public sealed class UpdateProjectCategoryCommandHandler : IRequestHandler<UpdateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    private readonly IProjectCategoryRepository _repository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCategoryCommandHandler(
        IProjectCategoryRepository repository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectCategoryResponse>> Handle(UpdateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
            {
                return Result<ProjectCategoryResponse>.Failure("Project category not found.");
            }

            var orgId = request.OrganizationId != Guid.Empty ? request.OrganizationId : category.OrganizationId;
            if (orgId == Guid.Empty)
            {
                var orgs = await _organizationRepository.GetAllAsync(cancellationToken);
                if (orgs != null && orgs.Count > 0)
                {
                    orgId = orgs[0].Id;
                }
            }

            var exists = await _repository.ExistsByNameAsync(orgId, request.Name, request.Id, cancellationToken);
            if (exists)
            {
                return Result<ProjectCategoryResponse>.Failure($"Project category name '{request.Name}' already exists.");
            }

            var updatedCmd = request with { OrganizationId = orgId };
            ProjectCategoryMapper.ApplyUpdate(updatedCmd, category);
            category.OrganizationId = orgId;
            category.UpdatedAtUtc = DateTime.UtcNow;

            _repository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ProjectCategoryResponse>.Success(ProjectCategoryMapper.ToResponse(category));
        }
        catch (Exception ex)
        {
            return Result<ProjectCategoryResponse>.Failure($"Error updating project category: {ex.Message}");
        }
    }
}
