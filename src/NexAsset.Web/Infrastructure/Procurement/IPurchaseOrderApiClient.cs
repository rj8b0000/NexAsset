using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Procurement;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Procurement
{
    /// <summary>
    /// Typed client for /api/enterprise-operations/purchase-orders. Workflow entity:
    /// create + status transitions only (the backend exposes no PUT/DELETE).
    /// </summary>
    public interface IPurchaseOrderApiClient
    {
        Task<ApiResult<PagedResult<PurchaseOrderItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<PurchaseOrderItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(PurchaseOrderFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> SetStatusAsync(Guid id, SetProcurementStatusRequest request, CancellationToken cancellationToken = default);
    }
}
