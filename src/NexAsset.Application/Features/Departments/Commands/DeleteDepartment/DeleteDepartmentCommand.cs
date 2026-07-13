using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Departments.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid Id)
    : IRequest<Result>;
