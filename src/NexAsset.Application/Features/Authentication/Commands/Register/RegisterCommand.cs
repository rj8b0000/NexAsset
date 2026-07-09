using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword)
    : IRequest<Result<RegisterResponse>>;