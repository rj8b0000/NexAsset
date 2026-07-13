using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Commands.DeleteEmployee;

public sealed record DeleteEmployeeCommand(Guid Id)
    : IRequest<Result>;
