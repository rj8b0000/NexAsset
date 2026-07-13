using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.DeleteDesignation;

public sealed record DeleteDesignationCommand(Guid Id)
    : IRequest<Result>;
