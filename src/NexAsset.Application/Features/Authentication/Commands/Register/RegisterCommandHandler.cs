using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterRequest(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        return _identityService.RegisterAsync(
            registerRequest,
            cancellationToken);
    }
}