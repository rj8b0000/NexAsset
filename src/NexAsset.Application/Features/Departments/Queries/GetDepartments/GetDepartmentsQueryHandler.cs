using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Queries.GetDepartments;

public sealed class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, Result<PagedResponse<DepartmentListItemResponse>>>
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentsQueryHandler(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<DepartmentListItemResponse>>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(
            request,
            cancellationToken);

        return Result<PagedResponse<DepartmentListItemResponse>>
            .Success(page.Map(DepartmentMapper.ToListItemResponse));
    }
}
