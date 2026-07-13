namespace NexAsset.Application.Features.AssetTransfers.Queries.GetAssetTransferHistory;

public sealed record AssetTransferResponse(
    Guid Id,
    Guid AssetId,
    Guid? FromEmployeeId,
    Guid? ToEmployeeId,
    Guid? FromBranchId,
    Guid? ToBranchId,
    Guid? FromDepartmentId,
    Guid? ToDepartmentId,
    DateOnly TransferDate,
    bool IsApproved,
    string? Remarks);
