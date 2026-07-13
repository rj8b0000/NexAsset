using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Foundation;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Foundation
{
    /// <summary>
    /// Real HTTP client for /api/organizations, replacing the former mock. Inherits the cookie
    /// auth + uniform error pipeline from <see cref="ApiClientBase"/>; declares only endpoints.
    /// </summary>
    public sealed class OrganizationApiClient : ApiClientBase, IOrganizationApiClient
    {
        private const string BasePath = "api/organizations";

        public OrganizationApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<OrganizationListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<OrganizationListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult<OrganizationDetail>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => GetAsync<OrganizationDetail>($"{BasePath}/{id}", cancellationToken);

        public Task<ApiResult<CreatedResponse>> CreateAsync(OrganizationFormModel model, CancellationToken cancellationToken = default)
            => PostAsync<OrganizationFormModel, CreatedResponse>(BasePath, model, cancellationToken);

        public Task<ApiResult> UpdateAsync(Guid id, OrganizationFormModel model, CancellationToken cancellationToken = default)
            => PutAsync($"{BasePath}/{id}", model, cancellationToken);

        public Task<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{id}", cancellationToken);
    }
}
