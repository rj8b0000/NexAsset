using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;

namespace NexAsset.Application.Features.AssetTransfers.Commands.TransferAsset;

public sealed record TransferAssetCommand(
    Guid AssetId,
    Guid? ToEmployeeId,
    Guid? ToBranchId,
    Guid? ToDepartmentId,
    DateOnly TransferDate,
    bool IsApproved,
    string? Remarks)
    : IRequest<Result<AssetTransferResponse>>;
