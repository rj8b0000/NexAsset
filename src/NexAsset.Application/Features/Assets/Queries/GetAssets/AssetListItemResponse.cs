using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Assets.Queries.GetAssets;

public sealed record AssetListItemResponse(
    Guid Id,
    Guid OrganizationId,
    Guid CategoryId,
    Guid? BranchId,
    Guid? DepartmentId,
    Guid? CurrentEmployeeId,
    string AssetCode,
    string AssetName,
    string? SerialNumber,
    string? Barcode,
    AssetStatus AssetStatus);
