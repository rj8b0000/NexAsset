using MediatR;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Assets.Queries.GetAsset;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Assets.Commands.UpdateAsset;

public sealed record UpdateAssetCommand(
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
    string? QrCode,
    string? Brand,
    string? Model,
    DateOnly? PurchaseDate,
    DateOnly? WarrantyExpiry,
    string? Vendor,
    decimal? PurchaseCost,
    decimal? CurrentValue,
    AssetStatus AssetStatus,
    string? Location,
    string? Remarks)
    : IRequest<Result<AssetResponse>>;
