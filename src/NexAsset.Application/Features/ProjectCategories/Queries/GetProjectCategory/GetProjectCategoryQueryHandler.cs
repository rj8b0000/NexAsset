using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategory;

public sealed class GetProjectCategoryQueryHandler : IRequestHandler<GetProjectCategoryQuery, Result<ProjectCategoryResponse>>
{
    private readonly IProjectCategoryRepository _repository;

    public GetProjectCategoryQueryHandler(IProjectCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProjectCategoryResponse>> Handle(GetProjectCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result<ProjectCategoryResponse>.Failure("Project category not found.");
        }

        return Result<ProjectCategoryResponse>.Success(ProjectCategoryMapper.ToResponse(category));
    }
}
