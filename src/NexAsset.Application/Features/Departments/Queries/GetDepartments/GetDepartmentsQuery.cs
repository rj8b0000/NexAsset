using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Queries.GetDepartments;

public sealed class GetDepartmentsQuery
    : PagedRequest, IRequest<Result<PagedResponse<DepartmentListItemResponse>>>;
