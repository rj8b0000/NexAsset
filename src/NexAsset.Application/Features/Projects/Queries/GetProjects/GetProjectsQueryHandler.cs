using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Result<PagedResponse<ProjectListItemResponse>>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<PagedResponse<ProjectListItemResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await _projectRepository.GetPagedAsync(
            request,
            request.Status,
            request.Priority,
            request.CategoryId,
            request.BranchId,
            request.DepartmentId,
            request.ProjectManagerEmployeeId,
            cancellationToken);

        var items = pagedResult.Items.Select(ProjectMapper.ToListItemResponse).ToList();

        var response = new PagedResponse<ProjectListItemResponse>
        {
            Items = items,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };

        return Result<PagedResponse<ProjectListItemResponse>>.Success(response);
    }
}
