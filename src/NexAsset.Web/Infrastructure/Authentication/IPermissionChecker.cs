namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Abstraction for future role/permission checks (see NexAsset.Application.Features.Permissions
    /// and .Roles on the backend). Not consulted by any page yet.
    /// </summary>
    public interface IPermissionChecker
    {
        bool HasPermission(string permission);
    }
}
