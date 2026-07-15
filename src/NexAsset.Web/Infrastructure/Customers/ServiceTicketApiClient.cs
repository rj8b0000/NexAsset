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
    /// <summary>Real HTTP client for /api/enterprise-operations/service-tickets.</summary>
    public sealed class ServiceTicketApiClient : ApiClientBase, IServiceTicketApiClient
    {
        private const string BasePath = "api/enterprise-operations/service-tickets";

        public ServiceTicketApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<ServiceTicketItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<ServiceTicketItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<ServiceTicketItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<ServiceTicketItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(ServiceTicketCreateModel model, CancellationToken cancellationToken = default)
            => PostAsync<ServiceTicketCreateModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, ServiceTicketUpdateModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);
    }
}
