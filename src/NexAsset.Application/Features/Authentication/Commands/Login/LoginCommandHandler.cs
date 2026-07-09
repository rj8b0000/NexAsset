using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IIdentityService _identityService;
    public LoginCommandHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginRequest = new LoginRequest(request.Email, request.Password, request.RememberMe);
        return _identityService.LoginAsync(
            loginRequest,
            cancellationToken);
    }
}