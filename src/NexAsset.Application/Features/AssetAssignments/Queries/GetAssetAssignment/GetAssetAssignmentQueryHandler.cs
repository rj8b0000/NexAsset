using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;

public sealed class GetAssetAssignmentQueryHandler : IRequestHandler<GetAssetAssignmentQuery, Result<AssetAssignmentResponse>>
{
    private readonly IAssetAssignmentRepository _repository;
    public GetAssetAssignmentQueryHandler(IAssetAssignmentRepository repository) => _repository = repository;

    public async Task<Result<AssetAssignmentResponse>> Handle(GetAssetAssignmentQuery request, CancellationToken cancellationToken)
    {
        var assignment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return assignment is null
            ? Result<AssetAssignmentResponse>.Failure("Asset assignment not found.")
            : Result<AssetAssignmentResponse>.Success(AssetWorkflowMapper.ToAssignmentResponse(assignment));
    }
}
