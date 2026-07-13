using NexAsset.Application.Features.AssetAssignments.Queries.GetAssetAssignment;
using NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;
using NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class AssetWorkflowMapper
{
    public static partial AssetAssignmentResponse ToAssignmentResponse(AssetAssignment assignment);
    public static partial AssetTransferResponse ToTransferResponse(AssetTransfer transfer);
    public static partial AssetReturnResponse ToReturnResponse(AssetReturn assetReturn);
}
