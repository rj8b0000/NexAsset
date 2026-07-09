namespace NexAsset.Application.Features.Authentication.Commands.Login;

public sealed record LoginResponse(
    Guid UserId,
    string Email,
    string FullName);