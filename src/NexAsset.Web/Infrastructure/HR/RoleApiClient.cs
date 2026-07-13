using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.HR;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.HR
{
    /// <summary>Real HTTP client for /api/roles.</summary>
    public sealed class RoleApiClient : ApiClientBase, IRoleApiClient
    {
        private const string BasePath = "api/roles";

        public RoleApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<RoleItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<RoleItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<RoleItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<RoleItem>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<RoleItem>> CreateAsync(RoleFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<RoleFormModel, RoleItem>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, RoleFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
