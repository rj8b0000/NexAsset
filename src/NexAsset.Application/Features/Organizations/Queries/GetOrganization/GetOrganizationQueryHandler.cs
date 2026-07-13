using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Queries.GetOrganization;

public sealed class GetOrganizationQueryHandler
    : IRequestHandler<GetOrganizationQuery, Result<GetOrganizationResponse>>
{
    private readonly IOrganizationRepository _repository;

    public GetOrganizationQueryHandler(
        IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetOrganizationResponse>> Handle(
        GetOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        var organization = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (organization is null)
        {
            return Result<GetOrganizationResponse>.Failure(
                "Organization not found.");
        }

        var response = OrganizationMapper.ToGetResponse(organization);

        return Result<GetOrganizationResponse>.Success(response);
    }
}