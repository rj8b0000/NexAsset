using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

public sealed class GetProjectCategoriesQueryHandler : IRequestHandler<GetProjectCategoriesQuery, Result<PagedResponse<ProjectCategoryResponse>>>
{
    private readonly IProjectCategoryRepository _repository;

    public GetProjectCategoriesQueryHandler(IProjectCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<ProjectCategoryResponse>>> Handle(GetProjectCategoriesQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _repository.GetPagedAsync(request, request.IsActive, cancellationToken);
        var responseItems = pagedResult.Items.Select(ProjectCategoryMapper.ToResponse).ToList();

        var response = new PagedResponse<ProjectCategoryResponse>
        {
            Items = responseItems,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };

        return Result<PagedResponse<ProjectCategoryResponse>>.Success(response);
    }
}
