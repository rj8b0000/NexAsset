namespace NexAsset.Application.Features.AssetReturns.Queries.GetAssetReturnHistory;

public sealed record AssetReturnResponse(
    Guid Id,
    Guid AssetId,
    Guid EmployeeId,
    DateOnly ReturnDate,
    string? InspectionNotes,
    string? ReturnRemarks,
    bool IsAssetUsable);
