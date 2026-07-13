using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployees;

public sealed class GetEmployeesQuery
    : PagedRequest, IRequest<Result<PagedResponse<EmployeeListItemResponse>>>;
