using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Customers;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Customers
{
    /// <summary>
    /// Typed client for /api/enterprise-operations/service-tickets: create + an update that
    /// changes assignment/priority/status/resolution/comments (no delete on the backend).
    /// </summary>
    public interface IServiceTicketApiClient
    {
        Task<ApiResult<PagedResult<ServiceTicketItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<ServiceTicketItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(ServiceTicketCreateModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, ServiceTicketUpdateModel model, CancellationToken cancellationToken = default);
    }
}
