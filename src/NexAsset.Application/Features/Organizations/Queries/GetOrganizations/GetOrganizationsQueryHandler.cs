using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Queries.GetOrganizations;

public sealed class GetOrganizationsQueryHandler
    : IRequestHandler<
        GetOrganizationsQuery,
        Result<PagedResponse<OrganizationListItemResponse>>>
{
    private readonly IOrganizationRepository _repository;

    public GetOrganizationsQueryHandler(
        IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<OrganizationListItemResponse>>> Handle(
        GetOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(
            request,
            cancellationToken);

        return Result<PagedResponse<OrganizationListItemResponse>>
            .Success(
                new PagedResponse<OrganizationListItemResponse>
                {
                    Items = page.Items
                        .Select(OrganizationMapper.ToListItemResponse)
                        .ToList(),
                    TotalCount = page.TotalCount,
                    PageNumber = page.PageNumber,
                    PageSize = page.PageSize
                });
    }
}