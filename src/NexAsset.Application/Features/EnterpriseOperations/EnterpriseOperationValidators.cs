using FluentValidation;

namespace NexAsset.Application.Features.EnterpriseOperations;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

public sealed class CreatePurchaseRequestCommandValidator : AbstractValidator<CreatePurchaseRequestCommand>
{
    public CreatePurchaseRequestCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.RequestNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RequestedByEmployeeId).NotEmpty();
        RuleFor(x => x.EstimatedAmount).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.OrderNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.VendorId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.ItemCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CurrentStock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReservedStock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReorderLevel).GreaterThanOrEqualTo(0);
    }
}

public sealed class StockMovementCommandValidator : AbstractValidator<StockMovementCommand>
{
    public StockMovementCommandValidator()
    {
        RuleFor(x => x.InventoryItemId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

public sealed class CreateConsumableCommandValidator : AbstractValidator<CreateConsumableCommand>
{
    public CreateConsumableCommandValidator()
    {
        RuleFor(x => x.InventoryItemId).NotEmpty();
        RuleFor(x => x.ConsumableCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public sealed class CreateMaintenanceRecordCommandValidator : AbstractValidator<CreateMaintenanceRecordCommand>
{
    public CreateMaintenanceRecordCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public sealed class CreateServiceTicketCommandValidator : AbstractValidator<CreateServiceTicketCommand>
{
    public CreateServiceTicketCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.TicketNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpsertSystemSettingCommandValidator : AbstractValidator<UpsertSystemSettingCommand>
{
    public UpsertSystemSettingCommandValidator()
    {
        RuleFor(x => x.Key).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Value).NotEmpty().MaximumLength(2000);
    }
}
