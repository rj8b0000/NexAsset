namespace NexAsset.Web.Models.Dashboard
{
    /// <summary>
    /// Mirrors NexAsset.Application's DashboardSummaryDto, returned by
    /// GET /api/enterprise-operations/dashboard/{organizationId}.
    /// </summary>
    public sealed record DashboardSummary(
        int Employees,
        int Assets,
        int AvailableAssets,
        int AssignedAssets,
        int LowStockItems,
        int PendingPurchaseRequests,
        int PendingPurchaseOrders,
        int OpenServiceTickets,
        int AssetsUnderMaintenance);
}
