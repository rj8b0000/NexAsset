using MediatR;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Queries.GetDesignations;

public sealed class GetDesignationsQuery
    : PagedRequest, IRequest<Result<PagedResponse<DesignationListItemResponse>>>;
