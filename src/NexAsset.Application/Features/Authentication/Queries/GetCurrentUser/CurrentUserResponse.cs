namespace NexAsset.Application.Features.Authentication.Queries.GetCurrentUser;

public sealed record CurrentUserResponse(
    Guid UserId,
    string Email,
    string UserName);