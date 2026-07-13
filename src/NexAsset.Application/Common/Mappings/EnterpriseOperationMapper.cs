using NexAsset.Application.Features.EnterpriseOperations;
using NexAsset.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace NexAsset.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class EnterpriseOperationMapper
{
    public static partial VendorDto ToDto(Vendor vendor);
    public static partial CustomerDto ToDto(Customer customer);
    public static partial PurchaseRequestDto ToDto(PurchaseRequest request);
    public static partial PurchaseOrderDto ToDto(PurchaseOrder order);
    public static partial InventoryItemDto ToDto(InventoryItem item);
    public static partial StockMovementDto ToDto(StockMovement movement);
    public static partial ConsumableDto ToDto(Consumable consumable);
    public static partial MaintenanceRecordDto ToDto(MaintenanceRecord record);
    public static partial ServiceTicketDto ToDto(ServiceTicket ticket);
    public static partial NotificationDto ToDto(Notification notification);
    public static partial AuditLogDto ToDto(AuditLog auditLog);
    public static partial SystemSettingDto ToDto(SystemSetting setting);
}
