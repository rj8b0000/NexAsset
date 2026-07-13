using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Queries.GetDesignations;

public sealed class GetDesignationsQueryHandler
    : IRequestHandler<GetDesignationsQuery, Result<PagedResponse<DesignationListItemResponse>>>
{
    private readonly IDesignationRepository _repository;

    public GetDesignationsQueryHandler(
        IDesignationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResponse<DesignationListItemResponse>>> Handle(
        GetDesignationsQuery request,
        CancellationToken cancellationToken)
    {
        var page = await _repository.GetPagedAsync(
            request,
            cancellationToken);

        return Result<PagedResponse<DesignationListItemResponse>>
            .Success(page.Map(DesignationMapper.ToListItemResponse));
    }
}
