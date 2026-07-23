using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Commands.UpdateProjectCategory;

public sealed class UpdateProjectCategoryCommandHandler : IRequestHandler<UpdateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    private readonly IProjectCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCategoryCommandHandler(IProjectCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectCategoryResponse>> Handle(UpdateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result<ProjectCategoryResponse>.Failure("Project category not found.");
        }

        var exists = await _repository.ExistsByNameAsync(request.OrganizationId, request.Name, request.Id, cancellationToken);
        if (exists)
        {
            return Result<ProjectCategoryResponse>.Failure($"Project category name '{request.Name}' already exists.");
        }

        ProjectCategoryMapper.ApplyUpdate(request, category);
        category.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProjectCategoryResponse>.Success(ProjectCategoryMapper.ToResponse(category));
    }
}
