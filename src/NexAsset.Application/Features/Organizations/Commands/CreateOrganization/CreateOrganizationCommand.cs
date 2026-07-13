using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Commands.CreateOrganization;

public sealed record CreateOrganizationCommand(
    string Code,
    string Name,
    string LegalName,
    string Email,
    string? Phone,
    string? Website,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string Currency,
    string TimeZone)
    : IRequest<Result<CreateOrganizationResponse>>;