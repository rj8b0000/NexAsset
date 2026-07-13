using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

namespace NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignmentHistory;

public sealed class GetAssetAssignmentHistoryQueryHandler
    : IRequestHandler<GetAssetAssignmentHistoryQuery, Result<IReadOnlyCollection<AssetAssignmentResponse>>>
{
    private readonly IAssetAssignmentRepository _repository;
    public GetAssetAssignmentHistoryQueryHandler(IAssetAssignmentRepository repository) => _repository = repository;

    public async Task<Result<IReadOnlyCollection<AssetAssignmentResponse>>> Handle(GetAssetAssignmentHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _repository.GetHistoryByAssetIdAsync(request.AssetId, cancellationToken);
        return Result<IReadOnlyCollection<AssetAssignmentResponse>>.Success(
            history.Select(AssetWorkflowMapper.ToAssignmentResponse).ToList());
    }
}
