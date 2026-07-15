using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Inventory;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Inventory
{
    /// <summary>Typed client for /api/enterprise-operations/consumables (no delete on the backend).</summary>
    public interface IConsumableApiClient
    {
        Task<ApiResult<PagedResult<ConsumableItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<ConsumableItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(ConsumableFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, ConsumableFormModel model, CancellationToken cancellationToken = default);
    }
}
