using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Models.Inventory;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Inventory
{
    /// <summary>Real HTTP client for /api/enterprise-operations/consumables.</summary>
    public sealed class ConsumableApiClient : ApiClientBase, IConsumableApiClient
    {
        private const string BasePath = "api/enterprise-operations/consumables";

        public ConsumableApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<ConsumableItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<ConsumableItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<ConsumableItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<ConsumableItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(ConsumableFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<ConsumableFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, ConsumableFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);
    }
}
