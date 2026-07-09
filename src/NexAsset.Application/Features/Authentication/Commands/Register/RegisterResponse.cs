namespace NexAsset.Application.Features.Authentication.Commands.Register;

public sealed record RegisterResponse(
    Guid UserId,
    string Email);