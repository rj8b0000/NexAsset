using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Queries.GetEmployee;

public sealed record GetEmployeeQuery(Guid Id)
    : IRequest<Result<GetEmployeeResponse>>;
