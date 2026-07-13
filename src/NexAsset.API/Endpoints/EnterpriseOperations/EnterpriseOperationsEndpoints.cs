using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.Application.Features.EnterpriseOperations;

namespace NexAsset.API.Endpoints.EnterpriseOperations;

public static class EnterpriseOperationsEndpoints
{
    public static IEndpointRouteBuilder MapEnterpriseOperationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/enterprise-operations")
            .WithTags("Enterprise Operations");

        MapVendors(group);
        MapCustomers(group);
        MapProcurement(group);
        MapInventory(group);
        MapMaintenance(group);
        MapServiceTickets(group);
        MapNotifications(group);
        MapAuditLogs(group);
        MapSystemSettings(group);
        MapDashboard(group);

        return app;
    }

    private static void MapVendors(RouteGroupBuilder group)
    {
        var vendors = group.MapGroup("/vendors");
        vendors.MapPost("/", Send<CreateVendorCommand>);
        vendors.MapGet("/", SendQuery<GetVendorsQuery>);
        vendors.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetVendorQuery(id))));
        vendors.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateVendorCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
        vendors.MapDelete("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteVendorCommand(id))));
    }

    private static void MapCustomers(RouteGroupBuilder group)
    {
        var customers = group.MapGroup("/customers");
        customers.MapPost("/", Send<CreateCustomerCommand>);
        customers.MapGet("/", SendQuery<GetCustomersQuery>);
        customers.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetCustomerQuery(id))));
        customers.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateCustomerCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
        customers.MapDelete("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new DeleteCustomerCommand(id))));
    }

    private static void MapProcurement(RouteGroupBuilder group)
    {
        var requests = group.MapGroup("/purchase-requests");
        requests.MapPost("/", Send<CreatePurchaseRequestCommand>);
        requests.MapGet("/", SendQuery<GetPurchaseRequestsQuery>);
        requests.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetPurchaseRequestQuery(id))));
        requests.MapPost("/{id:guid}/status", async (Guid id, [FromBody] SetPurchaseRequestStatusCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));

        var orders = group.MapGroup("/purchase-orders");
        orders.MapPost("/", Send<CreatePurchaseOrderCommand>);
        orders.MapGet("/", SendQuery<GetPurchaseOrdersQuery>);
        orders.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetPurchaseOrderQuery(id))));
        orders.MapPost("/{id:guid}/status", async (Guid id, [FromBody] SetPurchaseOrderStatusCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
    }

    private static void MapInventory(RouteGroupBuilder group)
    {
        var inventory = group.MapGroup("/inventory");
        inventory.MapPost("/", Send<CreateInventoryItemCommand>);
        inventory.MapGet("/", SendQuery<GetInventoryItemsQuery>);
        inventory.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetInventoryItemQuery(id))));
        inventory.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateInventoryItemCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
        inventory.MapPost("/stock-movements", Send<StockMovementCommand>);
        inventory.MapGet("/{inventoryItemId:guid}/stock-history", async (Guid inventoryItemId, ISender sender) =>
            ToResult(await sender.Send(new GetStockHistoryQuery(inventoryItemId))));

        var consumables = group.MapGroup("/consumables");
        consumables.MapPost("/", Send<CreateConsumableCommand>);
        consumables.MapGet("/", SendQuery<GetConsumablesQuery>);
        consumables.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetConsumableQuery(id))));
        consumables.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateConsumableCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
    }

    private static void MapMaintenance(RouteGroupBuilder group)
    {
        var maintenance = group.MapGroup("/maintenance");
        maintenance.MapPost("/", Send<CreateMaintenanceRecordCommand>);
        maintenance.MapGet("/", SendQuery<GetMaintenanceRecordsQuery>);
        maintenance.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetMaintenanceRecordQuery(id))));
        maintenance.MapPost("/{id:guid}/status", async (Guid id, [FromBody] UpdateMaintenanceStatusCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
    }

    private static void MapServiceTickets(RouteGroupBuilder group)
    {
        var tickets = group.MapGroup("/service-tickets");
        tickets.MapPost("/", Send<CreateServiceTicketCommand>);
        tickets.MapGet("/", SendQuery<GetServiceTicketsQuery>);
        tickets.MapGet("/{id:guid}", async (Guid id, ISender sender) => ToResult(await sender.Send(new GetServiceTicketQuery(id))));
        tickets.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateServiceTicketCommand body, ISender sender) =>
            ToResult(await sender.Send(body with { Id = id })));
    }

    private static void MapNotifications(RouteGroupBuilder group)
    {
        var notifications = group.MapGroup("/notifications");
        notifications.MapPost("/", Send<CreateNotificationCommand>);
        notifications.MapGet("/", SendQuery<GetNotificationsQuery>);
        notifications.MapPost("/{id:guid}/read", async (Guid id, ISender sender) => ToResult(await sender.Send(new MarkNotificationReadCommand(id))));
    }

    private static void MapAuditLogs(RouteGroupBuilder group)
    {
        var auditLogs = group.MapGroup("/audit-logs");
        auditLogs.MapPost("/", Send<CreateAuditLogCommand>);
        auditLogs.MapGet("/", SendQuery<GetAuditLogsQuery>);
    }

    private static void MapSystemSettings(RouteGroupBuilder group)
    {
        var settings = group.MapGroup("/system-settings");
        settings.MapPost("/", Send<UpsertSystemSettingCommand>);
        settings.MapGet("/", SendQuery<GetSystemSettingsQuery>);
    }

    private static void MapDashboard(RouteGroupBuilder group)
    {
        group.MapGet("/dashboard/{organizationId:guid}", async (Guid organizationId, ISender sender) =>
            ToResult(await sender.Send(new GetDashboardSummaryQuery(organizationId))));
    }

    private static async Task<IResult> Send<TCommand>(
        [FromBody] TCommand command,
        ISender sender)
        where TCommand : class
    {
        dynamic result = await sender.Send(command);
        return ToResult(result);
    }

    private static async Task<IResult> SendQuery<TQuery>(
        [AsParameters] TQuery query,
        ISender sender)
        where TQuery : class
    {
        dynamic result = await sender.Send(query);
        return ToResult(result);
    }

    private static IResult ToResult(dynamic result)
    {
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        if (result.Value is null)
            return Results.NoContent();

        return Results.Ok(result.Value);
    }
}
