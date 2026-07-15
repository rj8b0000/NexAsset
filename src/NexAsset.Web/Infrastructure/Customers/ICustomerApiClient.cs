using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Customers;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Customers
{
    /// <summary>Typed client for /api/enterprise-operations/customers (full CRUD).</summary>
    public interface ICustomerApiClient
    {
        Task<ApiResult<PagedResult<CustomerItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);
        Task<ApiResult<CustomerItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiResult<CreatedResponse>> CreateAsync(CustomerFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateAsync(Guid id, CustomerFormModel model, CancellationToken cancellationToken = default);
        Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
