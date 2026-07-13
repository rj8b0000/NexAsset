using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Commands.DeleteOrganization;

public sealed record DeleteOrganizationCommand(Guid Id)
    : IRequest<Result>;
