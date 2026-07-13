using System;

namespace NexAsset.Web.Models.Auth
{
    // Wire contracts for NexAsset.API's /api/auth endpoints. These are deliberately
    // hand-mirrored copies of the backend's request/response records — the frontend
    // talks to the backend over HTTP only and holds no project reference to it, so
    // these shapes must be kept in sync with:
    //   NexAsset.Application.Features.Authentication.Commands.Login.LoginCommand / LoginResponse
    //   NexAsset.Application.Features.Authentication.Queries.GetCurrentUser.CurrentUserResponse

    /// <summary>Body posted to <c>POST /api/auth/login</c>.</summary>
    public sealed record LoginRequest(string Email, string Password, bool RememberMe);

    /// <summary>Payload returned by <c>POST /api/auth/login</c> on success.</summary>
    public sealed record LoginResponse(Guid UserId, string Email, string FullName);

    /// <summary>Payload returned by <c>GET /api/auth/me</c> for the signed-in user.</summary>
    public sealed record CurrentUserResponse(Guid UserId, string Email, string UserName);

    /// <summary>
    /// Frontend-facing identity model. Normalizes the two backend shapes above
    /// (login returns <c>FullName</c>, /me returns <c>UserName</c>) onto a single
    /// <see cref="DisplayName"/> the UI and claims principal can rely on.
    /// </summary>
    public sealed record AuthenticatedUser(Guid UserId, string Email, string DisplayName)
    {
        public static AuthenticatedUser FromLogin(LoginResponse r) => new(r.UserId, r.Email, r.FullName);
        public static AuthenticatedUser FromCurrentUser(CurrentUserResponse r) => new(r.UserId, r.Email, r.UserName);
    }
}
