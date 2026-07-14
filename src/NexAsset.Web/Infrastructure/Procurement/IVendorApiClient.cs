using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Procurement;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Procurement
{
    /// <summary>Typed client for /api/enterprise-operations/vendors (full CRUD).</summary>
    public interface IVendorApiClient
    {
        Task<ApiResult<PagedResult<VendorItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<VendorItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(VendorFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, VendorFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
