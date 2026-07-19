using System;
using System.Threading;
using System.Threading.Tasks;
using NexAsset.Web.Infrastructure.API;
using NexAsset.Web.Models.Users;
using NexAsset.Web.Shared.Models;

namespace NexAsset.Web.Infrastructure.Users
{
    /// <summary>Client for /api/users — login-account administration.</summary>
    public interface IUserApiClient
    {
        Task<ApiResult<PagedResult<UserListItem>>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);

        Task<ApiResult> CreateAsync(UserFormModel model, CancellationToken cancellationToken = default);

        Task<ApiResult> AssignRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);

        Task<ApiResult> RemoveRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);

        Task<ApiResult> SetActiveAsync(Guid userId, bool isActive, CancellationToken cancellationToken = default);

        Task<ApiResult> LockAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<ApiResult> UnlockAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<ApiResult> ResetPasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default);
    }
}
