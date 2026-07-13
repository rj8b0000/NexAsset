using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployee;

public sealed class GetEmployeeQueryHandler
    : IRequestHandler<GetEmployeeQuery, Result<GetEmployeeResponse>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetEmployeeResponse>> Handle(
        GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return Result<GetEmployeeResponse>.Failure("Employee not found.");

        return Result<GetEmployeeResponse>.Success(
            EmployeeMapper.ToGetResponse(employee));
    }
}
