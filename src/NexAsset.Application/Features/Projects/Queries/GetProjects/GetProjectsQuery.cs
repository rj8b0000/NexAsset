using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectListItemResponse>>>
{
    public ProjectStatus? Status { get; init; }
    public ProjectPriority? Priority { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? BranchId { get; init; }
    public Guid? DepartmentId { get; init; }
    public Guid? ProjectManagerEmployeeId { get; init; }
}
