using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        return _identityService.ResetPasswordAsync(
            request.UserId,
            request.NewPassword,
            cancellationToken);
    }
}
