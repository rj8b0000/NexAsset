using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Customers;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Customers
{
    /// <summary>Real HTTP client for /api/enterprise-operations/customers.</summary>
    public sealed class CustomerApiClient : ApiClientBase, ICustomerApiClient
    {
        private const string BasePath = "api/enterprise-operations/customers";

        public CustomerApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<CustomerItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<CustomerItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<CustomerItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<CustomerItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(CustomerFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<CustomerFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, CustomerFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
