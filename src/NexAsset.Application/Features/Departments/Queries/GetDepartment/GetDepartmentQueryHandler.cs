using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Queries.GetDepartment;

public sealed class GetDepartmentQueryHandler
    : IRequestHandler<GetDepartmentQuery, Result<GetDepartmentResponse>>
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentQueryHandler(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetDepartmentResponse>> Handle(
        GetDepartmentQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
        {
            return Result<GetDepartmentResponse>
                .Failure("Department not found.");
        }

        return Result<GetDepartmentResponse>.Success(
            DepartmentMapper.ToGetResponse(department));
    }
}
