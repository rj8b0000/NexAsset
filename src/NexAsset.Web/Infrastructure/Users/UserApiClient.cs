using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Users;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Users
{
    /// <inheritdoc cref="IUserApiClient" />
    public sealed class UserApiClient : ApiClientBase, IUserApiClient
    {
        private const string BasePath = "api/users";

        public UserApiClient(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResult<PagedResult<UserListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default)
            => GetAsync<PagedResult<UserListItem>>(QueryStringBuilder.ForPagedQuery(BasePath, query), cancellationToken);

        public Task<ApiResult> CreateAsync(UserFormModel model, CancellationToken cancellationToken = default)
            => PostAsync(BasePath, new CreateUserRequest(model.Email, model.Password, model.OrganizationId, model.Roles), cancellationToken);

        public Task<ApiResult> AssignRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{userId}/roles", new AssignRoleRequest(roleName), cancellationToken);

        public Task<ApiResult> RemoveRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
            => DeleteAsync($"{BasePath}/{userId}/roles/{Uri.EscapeDataString(roleName)}", cancellationToken);

        public Task<ApiResult> SetActiveAsync(Guid userId, bool isActive, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{userId}/active", new SetActiveRequest(isActive), cancellationToken);

        public Task<ApiResult> LockAsync(Guid userId, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{userId}/lock", new { LockoutEnd = (DateTimeOffset?)DateTimeOffset.UtcNow.AddYears(100) }, cancellationToken);

        public Task<ApiResult> UnlockAsync(Guid userId, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{userId}/unlock", cancellationToken);

        public Task<ApiResult> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default)
            => PostAsync($"{BasePath}/{userId}/reset-password", new ResetPasswordRequest(newPassword), cancellationToken);
    }
}
