namespace NexAsset.Application.Features.Authentication.Commands.Register;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);