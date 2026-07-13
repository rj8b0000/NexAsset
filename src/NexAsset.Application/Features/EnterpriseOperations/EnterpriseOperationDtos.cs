using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.EnterpriseOperations;

public sealed record VendorDto(Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address, string? TaxNumber, bool IsActive);
public sealed record CustomerDto(Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address, bool IsActive);
public sealed record PurchaseRequestDto(Guid Id, Guid OrganizationId, string RequestNumber, string Title, Guid RequestedByEmployeeId, DateOnly RequestDate, ProcurementStatus Status, decimal EstimatedAmount, string? Description, string? ApprovalRemarks);
public sealed record PurchaseOrderDto(Guid Id, Guid OrganizationId, string OrderNumber, Guid? PurchaseRequestId, Guid VendorId, DateOnly OrderDate, DateOnly? ExpectedDeliveryDate, ProcurementStatus Status, decimal TotalAmount, string? Remarks);
public sealed record InventoryItemDto(Guid Id, Guid OrganizationId, Guid? BranchId, string ItemCode, string ItemName, int CurrentStock, int ReservedStock, int AvailableStock, int ReorderLevel, string UnitOfMeasure, bool IsActive);
public sealed record StockMovementDto(Guid Id, Guid InventoryItemId, StockMovementType MovementType, int Quantity, int StockAfterMovement, DateTime MovementAtUtc, string? ReferenceNumber, string? Remarks);
public sealed record ConsumableDto(Guid Id, Guid InventoryItemId, string ConsumableCode, string Name, string? Description, bool IsActive);
public sealed record MaintenanceRecordDto(Guid Id, Guid AssetId, MaintenanceType MaintenanceType, MaintenanceStatus Status, DateOnly RequestedDate, DateOnly? ScheduledDate, DateOnly? CompletedDate, string Title, string? Description, decimal? Cost, string? Remarks);
public sealed record ServiceTicketDto(Guid Id, Guid OrganizationId, Guid CustomerId, string TicketNumber, string Title, Guid? AssignedToEmployeeId, TicketPriority Priority, TicketStatus Status, string? Description, string? Resolution, string? Comments);
public sealed record NotificationDto(Guid Id, Guid? UserId, string Title, string Message, NotificationType NotificationType, bool IsRead, DateTime? ReadAtUtc);
public sealed record AuditLogDto(Guid Id, Guid? UserId, string EntityName, Guid? EntityId, string Action, string? OldValues, string? NewValues, DateTime TimestampUtc);
public sealed record SystemSettingDto(Guid Id, Guid? OrganizationId, string Key, string Value, string? Description, bool IsEncrypted);
public sealed record DashboardSummaryDto(int Employees, int Assets, int AvailableAssets, int AssignedAssets, int LowStockItems, int PendingPurchaseRequests, int PendingPurchaseOrders, int OpenServiceTickets, int AssetsUnderMaintenance);
