using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Queries.GetDepartment;

public sealed record GetDepartmentQuery(Guid Id)
    : IRequest<Result<GetDepartmentResponse>>;
