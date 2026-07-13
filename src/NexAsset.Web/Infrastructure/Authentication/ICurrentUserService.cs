using System;

namespace NexAsset.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Convenience, synchronous read-model of "who is signed in", derived from the
    /// <see cref="NexAssetAuthenticationStateProvider"/>'s claims principal. Feature code that
    /// just needs the current user's name/email can depend on this instead of awaiting an
    /// <c>AuthenticationState</c> or reaching into claims. For access decisions in markup,
    /// prefer <c>&lt;AuthorizeView&gt;</c>.
    /// </summary>
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        string? UserId { get; }
        string? Email { get; }
        string? DisplayName { get; }
        event Action? OnChange;
    }
}
