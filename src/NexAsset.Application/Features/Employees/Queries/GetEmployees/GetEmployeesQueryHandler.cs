using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployees;

public sealed class GetEmployeesQueryHandler
    : IRequestHandler<GetEmployeesQuery, Result<PagedResponse<EmployeeListItemResponse>>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeesQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<EmployeeListItemResponse>>> Handle(
        GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(request, cancellationToken);

        return Result<PagedResponse<EmployeeListItemResponse>>
            .Success(page.Map(EmployeeMapper.ToListItemResponse));
    }
}
