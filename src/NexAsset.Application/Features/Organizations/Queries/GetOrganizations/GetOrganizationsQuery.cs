using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Organizations.Queries.GetOrganizations;

public sealed class GetOrganizationsQuery
    : PagedRequest, IRequest<Result<PagedResponse<OrganizationListItemResponse>>>;