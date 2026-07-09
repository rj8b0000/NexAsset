using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Authentication.Commands.Login;
using NexAsset.Application.Features.Authentication.Commands.Register;

namespace NexAsset.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken
    );
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result> LogoutAsync();
}