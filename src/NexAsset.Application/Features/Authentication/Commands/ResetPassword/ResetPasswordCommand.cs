using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string NewPassword)
    : IRequest<Result>;
