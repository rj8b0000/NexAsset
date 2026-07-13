using FluentValidation;
using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.EnterpriseOperations;

public sealed record CreateVendorCommand(Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address, string? TaxNumber) : IRequest<Result<VendorDto>>;
public sealed record UpdateVendorCommand(Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address, string? TaxNumber, bool IsActive) : IRequest<Result<VendorDto>>;
public sealed record DeleteVendorCommand(Guid Id) : IRequest<Result>;
public sealed record GetVendorQuery(Guid Id) : IRequest<Result<VendorDto>>;
public sealed class GetVendorsQuery : PagedRequest, IRequest<Result<PagedResponse<VendorDto>>>;

public sealed record CreateCustomerCommand(Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address) : IRequest<Result<CustomerDto>>;
public sealed record UpdateCustomerCommand(Guid Id, Guid OrganizationId, string Code, string Name, string? ContactPerson, string Email, string? Phone, string? Address, bool IsActive) : IRequest<Result<CustomerDto>>;
public sealed record DeleteCustomerCommand(Guid Id) : IRequest<Result>;
public sealed record GetCustomerQuery(Guid Id) : IRequest<Result<CustomerDto>>;
public sealed class GetCustomersQuery : PagedRequest, IRequest<Result<PagedResponse<CustomerDto>>>;

public sealed record CreatePurchaseRequestCommand(Guid OrganizationId, string RequestNumber, string Title, string? Description, Guid RequestedByEmployeeId, DateOnly RequestDate, decimal EstimatedAmount) : IRequest<Result<PurchaseRequestDto>>;
public sealed record SetPurchaseRequestStatusCommand(Guid Id, ProcurementStatus Status, Guid? ApprovedByEmployeeId, string? ApprovalRemarks) : IRequest<Result<PurchaseRequestDto>>;
public sealed record GetPurchaseRequestQuery(Guid Id) : IRequest<Result<PurchaseRequestDto>>;
public sealed class GetPurchaseRequestsQuery : PagedRequest, IRequest<Result<PagedResponse<PurchaseRequestDto>>>;

public sealed record CreatePurchaseOrderCommand(Guid OrganizationId, string OrderNumber, Guid? PurchaseRequestId, Guid VendorId, DateOnly OrderDate, DateOnly? ExpectedDeliveryDate, decimal TotalAmount, string? Remarks) : IRequest<Result<PurchaseOrderDto>>;
public sealed record SetPurchaseOrderStatusCommand(Guid Id, ProcurementStatus Status, Guid? ApprovedByEmployeeId, string? Remarks) : IRequest<Result<PurchaseOrderDto>>;
public sealed record GetPurchaseOrderQuery(Guid Id) : IRequest<Result<PurchaseOrderDto>>;
public sealed class GetPurchaseOrdersQuery : PagedRequest, IRequest<Result<PagedResponse<PurchaseOrderDto>>>;

public sealed record CreateInventoryItemCommand(Guid OrganizationId, Guid? BranchId, string ItemCode, string ItemName, string? Description, int CurrentStock, int ReservedStock, int ReorderLevel, string UnitOfMeasure) : IRequest<Result<InventoryItemDto>>;
public sealed record UpdateInventoryItemCommand(Guid Id, Guid OrganizationId, Guid? BranchId, string ItemCode, string ItemName, string? Description, int CurrentStock, int ReservedStock, int ReorderLevel, string UnitOfMeasure, bool IsActive) : IRequest<Result<InventoryItemDto>>;
public sealed record GetInventoryItemQuery(Guid Id) : IRequest<Result<InventoryItemDto>>;
public sealed class GetInventoryItemsQuery : PagedRequest, IRequest<Result<PagedResponse<InventoryItemDto>>>;
public sealed record StockMovementCommand(Guid InventoryItemId, StockMovementType MovementType, int Quantity, string? ReferenceNumber, string? Remarks) : IRequest<Result<StockMovementDto>>;
public sealed record GetStockHistoryQuery(Guid InventoryItemId) : IRequest<Result<IReadOnlyCollection<StockMovementDto>>>;

public sealed record CreateConsumableCommand(Guid InventoryItemId, string ConsumableCode, string Name, string? Description) : IRequest<Result<ConsumableDto>>;
public sealed record UpdateConsumableCommand(Guid Id, Guid InventoryItemId, string ConsumableCode, string Name, string? Description, bool IsActive) : IRequest<Result<ConsumableDto>>;
public sealed record GetConsumableQuery(Guid Id) : IRequest<Result<ConsumableDto>>;
public sealed class GetConsumablesQuery : PagedRequest, IRequest<Result<PagedResponse<ConsumableDto>>>;

public sealed record CreateMaintenanceRecordCommand(Guid AssetId, MaintenanceType MaintenanceType, DateOnly RequestedDate, DateOnly? ScheduledDate, string Title, string? Description, decimal? Cost, string? Remarks) : IRequest<Result<MaintenanceRecordDto>>;
public sealed record UpdateMaintenanceStatusCommand(Guid Id, MaintenanceStatus Status, DateOnly? ScheduledDate, DateOnly? CompletedDate, string? Remarks) : IRequest<Result<MaintenanceRecordDto>>;
public sealed record GetMaintenanceRecordQuery(Guid Id) : IRequest<Result<MaintenanceRecordDto>>;
public sealed class GetMaintenanceRecordsQuery : PagedRequest, IRequest<Result<PagedResponse<MaintenanceRecordDto>>>;

public sealed record CreateServiceTicketCommand(Guid OrganizationId, Guid CustomerId, string TicketNumber, string Title, string? Description, TicketPriority Priority) : IRequest<Result<ServiceTicketDto>>;
public sealed record UpdateServiceTicketCommand(Guid Id, Guid? AssignedToEmployeeId, TicketPriority Priority, TicketStatus Status, string? Resolution, string? Comments) : IRequest<Result<ServiceTicketDto>>;
public sealed record GetServiceTicketQuery(Guid Id) : IRequest<Result<ServiceTicketDto>>;
public sealed class GetServiceTicketsQuery : PagedRequest, IRequest<Result<PagedResponse<ServiceTicketDto>>>;

public sealed record CreateNotificationCommand(Guid? UserId, string Title, string Message, NotificationType NotificationType) : IRequest<Result<NotificationDto>>;
public sealed record MarkNotificationReadCommand(Guid Id) : IRequest<Result<NotificationDto>>;
public sealed class GetNotificationsQuery : PagedRequest, IRequest<Result<PagedResponse<NotificationDto>>>;

public sealed record CreateAuditLogCommand(Guid? UserId, string EntityName, Guid? EntityId, string Action, string? OldValues, string? NewValues) : IRequest<Result<AuditLogDto>>;
public sealed class GetAuditLogsQuery : PagedRequest, IRequest<Result<PagedResponse<AuditLogDto>>>;

public sealed record UpsertSystemSettingCommand(Guid? OrganizationId, string Key, string Value, string? Description, bool IsEncrypted) : IRequest<Result<SystemSettingDto>>;
public sealed class GetSystemSettingsQuery : PagedRequest, IRequest<Result<PagedResponse<SystemSettingDto>>>;
public sealed record GetDashboardSummaryQuery(Guid OrganizationId) : IRequest<Result<DashboardSummaryDto>>;

public sealed class CreateVendorCommandValidator :
    AbstractValidator<CreateVendorCommand>
{
    public CreateVendorCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

public sealed class EnterpriseOperationHandler :
    IRequestHandler<CreateVendorCommand, Result<VendorDto>>,
    IRequestHandler<UpdateVendorCommand, Result<VendorDto>>,
    IRequestHandler<DeleteVendorCommand, Result>,
    IRequestHandler<GetVendorQuery, Result<VendorDto>>,
    IRequestHandler<GetVendorsQuery, Result<PagedResponse<VendorDto>>>,
    IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>,
    IRequestHandler<UpdateCustomerCommand, Result<CustomerDto>>,
    IRequestHandler<DeleteCustomerCommand, Result>,
    IRequestHandler<GetCustomerQuery, Result<CustomerDto>>,
    IRequestHandler<GetCustomersQuery, Result<PagedResponse<CustomerDto>>>,
    IRequestHandler<CreatePurchaseRequestCommand, Result<PurchaseRequestDto>>,
    IRequestHandler<SetPurchaseRequestStatusCommand, Result<PurchaseRequestDto>>,
    IRequestHandler<GetPurchaseRequestQuery, Result<PurchaseRequestDto>>,
    IRequestHandler<GetPurchaseRequestsQuery, Result<PagedResponse<PurchaseRequestDto>>>,
    IRequestHandler<CreatePurchaseOrderCommand, Result<PurchaseOrderDto>>,
    IRequestHandler<SetPurchaseOrderStatusCommand, Result<PurchaseOrderDto>>,
    IRequestHandler<GetPurchaseOrderQuery, Result<PurchaseOrderDto>>,
    IRequestHandler<GetPurchaseOrdersQuery, Result<PagedResponse<PurchaseOrderDto>>>,
    IRequestHandler<CreateInventoryItemCommand, Result<InventoryItemDto>>,
    IRequestHandler<UpdateInventoryItemCommand, Result<InventoryItemDto>>,
    IRequestHandler<GetInventoryItemQuery, Result<InventoryItemDto>>,
    IRequestHandler<GetInventoryItemsQuery, Result<PagedResponse<InventoryItemDto>>>,
    IRequestHandler<StockMovementCommand, Result<StockMovementDto>>,
    IRequestHandler<GetStockHistoryQuery, Result<IReadOnlyCollection<StockMovementDto>>>,
    IRequestHandler<CreateConsumableCommand, Result<ConsumableDto>>,
    IRequestHandler<UpdateConsumableCommand, Result<ConsumableDto>>,
    IRequestHandler<GetConsumableQuery, Result<ConsumableDto>>,
    IRequestHandler<GetConsumablesQuery, Result<PagedResponse<ConsumableDto>>>,
    IRequestHandler<CreateMaintenanceRecordCommand, Result<MaintenanceRecordDto>>,
    IRequestHandler<UpdateMaintenanceStatusCommand, Result<MaintenanceRecordDto>>,
    IRequestHandler<GetMaintenanceRecordQuery, Result<MaintenanceRecordDto>>,
    IRequestHandler<GetMaintenanceRecordsQuery, Result<PagedResponse<MaintenanceRecordDto>>>,
    IRequestHandler<CreateServiceTicketCommand, Result<ServiceTicketDto>>,
    IRequestHandler<UpdateServiceTicketCommand, Result<ServiceTicketDto>>,
    IRequestHandler<GetServiceTicketQuery, Result<ServiceTicketDto>>,
    IRequestHandler<GetServiceTicketsQuery, Result<PagedResponse<ServiceTicketDto>>>,
    IRequestHandler<CreateNotificationCommand, Result<NotificationDto>>,
    IRequestHandler<MarkNotificationReadCommand, Result<NotificationDto>>,
    IRequestHandler<GetNotificationsQuery, Result<PagedResponse<NotificationDto>>>,
    IRequestHandler<CreateAuditLogCommand, Result<AuditLogDto>>,
    IRequestHandler<GetAuditLogsQuery, Result<PagedResponse<AuditLogDto>>>,
    IRequestHandler<UpsertSystemSettingCommand, Result<SystemSettingDto>>,
    IRequestHandler<GetSystemSettingsQuery, Result<PagedResponse<SystemSettingDto>>>,
    IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
{
    private readonly IEnterpriseOperationsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public EnterpriseOperationHandler(IEnterpriseOperationsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<VendorDto>> Handle(CreateVendorCommand r, CancellationToken ct)
    {
        var entity = new Vendor { OrganizationId = r.OrganizationId, Code = r.Code, Name = r.Name, ContactPerson = r.ContactPerson, Email = r.Email, Phone = r.Phone, Address = r.Address, TaxNumber = r.TaxNumber, IsActive = true };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<VendorDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<VendorDto>> Handle(UpdateVendorCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Vendor>(r.Id, ct);
        if (entity is null) return Result<VendorDto>.Failure("Vendor not found.");
        entity.OrganizationId = r.OrganizationId; entity.Code = r.Code; entity.Name = r.Name; entity.ContactPerson = r.ContactPerson; entity.Email = r.Email; entity.Phone = r.Phone; entity.Address = r.Address; entity.TaxNumber = r.TaxNumber; entity.IsActive = r.IsActive; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<VendorDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteVendorCommand r, CancellationToken ct) => await SoftDelete<Vendor>(r.Id, ct, "Vendor not found.");
    public async Task<Result<VendorDto>> Handle(GetVendorQuery r, CancellationToken ct) => await Get<Vendor, VendorDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Vendor not found.");
    public async Task<Result<PagedResponse<VendorDto>>> Handle(GetVendorsQuery r, CancellationToken ct) => await Page<Vendor, VendorDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand r, CancellationToken ct)
    {
        var entity = new Customer { OrganizationId = r.OrganizationId, Code = r.Code, Name = r.Name, ContactPerson = r.ContactPerson, Email = r.Email, Phone = r.Phone, Address = r.Address, IsActive = true };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<CustomerDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Customer>(r.Id, ct);
        if (entity is null) return Result<CustomerDto>.Failure("Customer not found.");
        entity.OrganizationId = r.OrganizationId; entity.Code = r.Code; entity.Name = r.Name; entity.ContactPerson = r.ContactPerson; entity.Email = r.Email; entity.Phone = r.Phone; entity.Address = r.Address; entity.IsActive = r.IsActive; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<CustomerDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteCustomerCommand r, CancellationToken ct) => await SoftDelete<Customer>(r.Id, ct, "Customer not found.");
    public async Task<Result<CustomerDto>> Handle(GetCustomerQuery r, CancellationToken ct) => await Get<Customer, CustomerDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Customer not found.");
    public async Task<Result<PagedResponse<CustomerDto>>> Handle(GetCustomersQuery r, CancellationToken ct) => await Page<Customer, CustomerDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<PurchaseRequestDto>> Handle(CreatePurchaseRequestCommand r, CancellationToken ct)
    {
        var entity = new PurchaseRequest { OrganizationId = r.OrganizationId, RequestNumber = r.RequestNumber, Title = r.Title, Description = r.Description, RequestedByEmployeeId = r.RequestedByEmployeeId, RequestDate = r.RequestDate, EstimatedAmount = r.EstimatedAmount, Status = ProcurementStatus.PendingApproval };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<PurchaseRequestDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PurchaseRequestDto>> Handle(SetPurchaseRequestStatusCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<PurchaseRequest>(r.Id, ct);
        if (entity is null) return Result<PurchaseRequestDto>.Failure("Purchase request not found.");
        entity.Status = r.Status; entity.ApprovedByEmployeeId = r.ApprovedByEmployeeId; entity.ApprovalRemarks = r.ApprovalRemarks; entity.ApprovedAtUtc = r.Status == ProcurementStatus.Approved ? DateTime.UtcNow : entity.ApprovedAtUtc; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<PurchaseRequestDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PurchaseRequestDto>> Handle(GetPurchaseRequestQuery r, CancellationToken ct) => await Get<PurchaseRequest, PurchaseRequestDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Purchase request not found.");
    public async Task<Result<PagedResponse<PurchaseRequestDto>>> Handle(GetPurchaseRequestsQuery r, CancellationToken ct) => await Page<PurchaseRequest, PurchaseRequestDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<PurchaseOrderDto>> Handle(CreatePurchaseOrderCommand r, CancellationToken ct)
    {
        var entity = new PurchaseOrder { OrganizationId = r.OrganizationId, OrderNumber = r.OrderNumber, PurchaseRequestId = r.PurchaseRequestId, VendorId = r.VendorId, OrderDate = r.OrderDate, ExpectedDeliveryDate = r.ExpectedDeliveryDate, TotalAmount = r.TotalAmount, Remarks = r.Remarks, Status = ProcurementStatus.PendingApproval };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<PurchaseOrderDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PurchaseOrderDto>> Handle(SetPurchaseOrderStatusCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<PurchaseOrder>(r.Id, ct);
        if (entity is null) return Result<PurchaseOrderDto>.Failure("Purchase order not found.");
        entity.Status = r.Status; entity.ApprovedByEmployeeId = r.ApprovedByEmployeeId; entity.Remarks = r.Remarks ?? entity.Remarks; entity.ApprovedAtUtc = r.Status == ProcurementStatus.Approved ? DateTime.UtcNow : entity.ApprovedAtUtc; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<PurchaseOrderDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PurchaseOrderDto>> Handle(GetPurchaseOrderQuery r, CancellationToken ct) => await Get<PurchaseOrder, PurchaseOrderDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Purchase order not found.");
    public async Task<Result<PagedResponse<PurchaseOrderDto>>> Handle(GetPurchaseOrdersQuery r, CancellationToken ct) => await Page<PurchaseOrder, PurchaseOrderDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<InventoryItemDto>> Handle(CreateInventoryItemCommand r, CancellationToken ct)
    {
        var entity = new InventoryItem { OrganizationId = r.OrganizationId, BranchId = r.BranchId, ItemCode = r.ItemCode, ItemName = r.ItemName, Description = r.Description, CurrentStock = r.CurrentStock, ReservedStock = r.ReservedStock, ReorderLevel = r.ReorderLevel, UnitOfMeasure = r.UnitOfMeasure, IsActive = true };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<InventoryItemDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<InventoryItemDto>> Handle(UpdateInventoryItemCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<InventoryItem>(r.Id, ct);
        if (entity is null) return Result<InventoryItemDto>.Failure("Inventory item not found.");
        entity.OrganizationId = r.OrganizationId; entity.BranchId = r.BranchId; entity.ItemCode = r.ItemCode; entity.ItemName = r.ItemName; entity.Description = r.Description; entity.CurrentStock = r.CurrentStock; entity.ReservedStock = r.ReservedStock; entity.ReorderLevel = r.ReorderLevel; entity.UnitOfMeasure = r.UnitOfMeasure; entity.IsActive = r.IsActive; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<InventoryItemDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<InventoryItemDto>> Handle(GetInventoryItemQuery r, CancellationToken ct) => await Get<InventoryItem, InventoryItemDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Inventory item not found.");
    public async Task<Result<PagedResponse<InventoryItemDto>>> Handle(GetInventoryItemsQuery r, CancellationToken ct) => await Page<InventoryItem, InventoryItemDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<StockMovementDto>> Handle(StockMovementCommand r, CancellationToken ct)
    {
        var item = await _repository.GetInventoryItemAsync(r.InventoryItemId, ct);
        if (item is null) return Result<StockMovementDto>.Failure("Inventory item not found.");
        var delta = r.MovementType == StockMovementType.StockOut ? -r.Quantity : r.Quantity;
        item.CurrentStock += delta; item.UpdatedAtUtc = DateTime.UtcNow;
        var movement = new StockMovement { InventoryItemId = item.Id, MovementType = r.MovementType, Quantity = r.Quantity, StockAfterMovement = item.CurrentStock, ReferenceNumber = r.ReferenceNumber, Remarks = r.Remarks };
        await _repository.AddAsync(movement, ct); _repository.Update(item); await _unitOfWork.SaveChangesAsync(ct);
        return Result<StockMovementDto>.Success(EnterpriseOperationMapper.ToDto(movement));
    }

    public async Task<Result<IReadOnlyCollection<StockMovementDto>>> Handle(GetStockHistoryQuery r, CancellationToken ct)
    {
        var history = await _repository.GetStockHistoryAsync(r.InventoryItemId, ct);
        return Result<IReadOnlyCollection<StockMovementDto>>.Success(history.Select(EnterpriseOperationMapper.ToDto).ToList());
    }

    public async Task<Result<ConsumableDto>> Handle(CreateConsumableCommand r, CancellationToken ct)
    {
        var entity = new Consumable { InventoryItemId = r.InventoryItemId, ConsumableCode = r.ConsumableCode, Name = r.Name, Description = r.Description, IsActive = true };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ConsumableDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<ConsumableDto>> Handle(UpdateConsumableCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Consumable>(r.Id, ct);
        if (entity is null) return Result<ConsumableDto>.Failure("Consumable not found.");
        entity.InventoryItemId = r.InventoryItemId; entity.ConsumableCode = r.ConsumableCode; entity.Name = r.Name; entity.Description = r.Description; entity.IsActive = r.IsActive; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ConsumableDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<ConsumableDto>> Handle(GetConsumableQuery r, CancellationToken ct) => await Get<Consumable, ConsumableDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Consumable not found.");
    public async Task<Result<PagedResponse<ConsumableDto>>> Handle(GetConsumablesQuery r, CancellationToken ct) => await Page<Consumable, ConsumableDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<MaintenanceRecordDto>> Handle(CreateMaintenanceRecordCommand r, CancellationToken ct)
    {
        var entity = new MaintenanceRecord { AssetId = r.AssetId, MaintenanceType = r.MaintenanceType, RequestedDate = r.RequestedDate, ScheduledDate = r.ScheduledDate, Title = r.Title, Description = r.Description, Cost = r.Cost, Remarks = r.Remarks, Status = r.ScheduledDate.HasValue ? MaintenanceStatus.Scheduled : MaintenanceStatus.Requested };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<MaintenanceRecordDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<MaintenanceRecordDto>> Handle(UpdateMaintenanceStatusCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<MaintenanceRecord>(r.Id, ct);
        if (entity is null) return Result<MaintenanceRecordDto>.Failure("Maintenance record not found.");
        entity.Status = r.Status; entity.ScheduledDate = r.ScheduledDate ?? entity.ScheduledDate; entity.CompletedDate = r.CompletedDate ?? entity.CompletedDate; entity.Remarks = r.Remarks ?? entity.Remarks; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<MaintenanceRecordDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<MaintenanceRecordDto>> Handle(GetMaintenanceRecordQuery r, CancellationToken ct) => await Get<MaintenanceRecord, MaintenanceRecordDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Maintenance record not found.");
    public async Task<Result<PagedResponse<MaintenanceRecordDto>>> Handle(GetMaintenanceRecordsQuery r, CancellationToken ct) => await Page<MaintenanceRecord, MaintenanceRecordDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<ServiceTicketDto>> Handle(CreateServiceTicketCommand r, CancellationToken ct)
    {
        var entity = new ServiceTicket { OrganizationId = r.OrganizationId, CustomerId = r.CustomerId, TicketNumber = r.TicketNumber, Title = r.Title, Description = r.Description, Priority = r.Priority, Status = TicketStatus.Open };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ServiceTicketDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<ServiceTicketDto>> Handle(UpdateServiceTicketCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ServiceTicket>(r.Id, ct);
        if (entity is null) return Result<ServiceTicketDto>.Failure("Service ticket not found.");
        entity.AssignedToEmployeeId = r.AssignedToEmployeeId; entity.Priority = r.Priority; entity.Status = r.Status; entity.Resolution = r.Resolution; entity.Comments = r.Comments; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ServiceTicketDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<ServiceTicketDto>> Handle(GetServiceTicketQuery r, CancellationToken ct) => await Get<ServiceTicket, ServiceTicketDto>(r.Id, EnterpriseOperationMapper.ToDto, ct, "Service ticket not found.");
    public async Task<Result<PagedResponse<ServiceTicketDto>>> Handle(GetServiceTicketsQuery r, CancellationToken ct) => await Page<ServiceTicket, ServiceTicketDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<NotificationDto>> Handle(CreateNotificationCommand r, CancellationToken ct)
    {
        var entity = new Notification { UserId = r.UserId, Title = r.Title, Message = r.Message, NotificationType = r.NotificationType };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<NotificationDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<NotificationDto>> Handle(MarkNotificationReadCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Notification>(r.Id, ct);
        if (entity is null) return Result<NotificationDto>.Failure("Notification not found.");
        entity.IsRead = true; entity.ReadAtUtc = DateTime.UtcNow; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<NotificationDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PagedResponse<NotificationDto>>> Handle(GetNotificationsQuery r, CancellationToken ct) => await Page<Notification, NotificationDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<AuditLogDto>> Handle(CreateAuditLogCommand r, CancellationToken ct)
    {
        var entity = new AuditLog { UserId = r.UserId, EntityName = r.EntityName, EntityId = r.EntityId, Action = r.Action, OldValues = r.OldValues, NewValues = r.NewValues };
        await _repository.AddAsync(entity, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<AuditLogDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PagedResponse<AuditLogDto>>> Handle(GetAuditLogsQuery r, CancellationToken ct) => await Page<AuditLog, AuditLogDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public async Task<Result<SystemSettingDto>> Handle(UpsertSystemSettingCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetSystemSettingAsync(r.OrganizationId, r.Key, ct);

        if (entity is null)
        {
            entity = new SystemSetting { OrganizationId = r.OrganizationId, Key = r.Key, Value = r.Value, Description = r.Description, IsEncrypted = r.IsEncrypted };
            await _repository.AddAsync(entity, ct);
        }
        else
        {
            entity.Value = r.Value;
            entity.Description = r.Description;
            entity.IsEncrypted = r.IsEncrypted;
            entity.UpdatedAtUtc = DateTime.UtcNow;
            _repository.Update(entity);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return Result<SystemSettingDto>.Success(EnterpriseOperationMapper.ToDto(entity));
    }

    public async Task<Result<PagedResponse<SystemSettingDto>>> Handle(GetSystemSettingsQuery r, CancellationToken ct) => await Page<SystemSetting, SystemSettingDto>(r, EnterpriseOperationMapper.ToDto, ct);

    public Task<Result<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery r, CancellationToken ct)
    {
        return BuildDashboardAsync(r.OrganizationId, ct);
    }

    private async Task<Result<DashboardSummaryDto>> BuildDashboardAsync(Guid organizationId, CancellationToken ct)
    {
        var employees = await _repository.CountAsync<Employee>(q => q.Where(x => x.OrganizationId == organizationId && !x.IsDeleted), ct);
        var assets = await _repository.CountAsync<Asset>(q => q.Where(x => x.OrganizationId == organizationId && !x.IsDeleted), ct);
        var availableAssets = await _repository.CountAsync<Asset>(q => q.Where(x => x.OrganizationId == organizationId && x.AssetStatus == AssetStatus.Available && !x.IsDeleted), ct);
        var assignedAssets = await _repository.CountAsync<Asset>(q => q.Where(x => x.OrganizationId == organizationId && x.AssetStatus == AssetStatus.Assigned && !x.IsDeleted), ct);
        var lowStock = await _repository.CountAsync<InventoryItem>(q => q.Where(x => x.OrganizationId == organizationId && x.CurrentStock <= x.ReorderLevel && !x.IsDeleted), ct);
        var pendingRequests = await _repository.CountAsync<PurchaseRequest>(q => q.Where(x => x.OrganizationId == organizationId && x.Status == ProcurementStatus.PendingApproval && !x.IsDeleted), ct);
        var pendingOrders = await _repository.CountAsync<PurchaseOrder>(q => q.Where(x => x.OrganizationId == organizationId && x.Status == ProcurementStatus.PendingApproval && !x.IsDeleted), ct);
        var openTickets = await _repository.CountAsync<ServiceTicket>(q => q.Where(x => x.OrganizationId == organizationId && x.Status != TicketStatus.Closed && !x.IsDeleted), ct);
        var maintenance = await _repository.CountAsync<MaintenanceRecord>(q => q.Where(x => x.Status == MaintenanceStatus.InProgress && !x.IsDeleted), ct);

        return Result<DashboardSummaryDto>.Success(
            new DashboardSummaryDto(
                employees,
                assets,
                availableAssets,
                assignedAssets,
                lowStock,
                pendingRequests,
                pendingOrders,
                openTickets,
                maintenance));
    }

    private async Task<Result> SoftDelete<TEntity>(Guid id, CancellationToken ct, string notFound)
        where TEntity : NexAsset.Domain.Common.BaseEntity
    {
        var entity = await _repository.GetByIdAsync<TEntity>(id, ct);
        if (entity is null) return Result.Failure(notFound);
        entity.IsDeleted = true; entity.DeletedAtUtc = DateTime.UtcNow; entity.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    private async Task<Result<TDto>> Get<TEntity, TDto>(Guid id, Func<TEntity, TDto> mapper, CancellationToken ct, string notFound)
        where TEntity : class
    {
        var entity = await _repository.GetByIdAsync<TEntity>(id, ct);
        return entity is null ? Result<TDto>.Failure(notFound) : Result<TDto>.Success(mapper(entity));
    }

    private async Task<Result<PagedResponse<TDto>>> Page<TEntity, TDto>(PagedRequest request, Func<TEntity, TDto> mapper, CancellationToken ct)
        where TEntity : class
    {
        var page = await _repository.GetPagedAsync<TEntity>(request, ct);
        return Result<PagedResponse<TDto>>.Success(page.Map(mapper));
    }
}
