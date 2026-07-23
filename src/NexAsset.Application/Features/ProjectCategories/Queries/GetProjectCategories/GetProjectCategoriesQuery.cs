using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

public sealed class GetProjectCategoriesQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectCategoryResponse>>>
{
    public bool? IsActive { get; init; }
}
