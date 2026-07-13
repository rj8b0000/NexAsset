using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
{
    private readonly ICurrentUserService _currentUserService;
    public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            return Task.FromResult(
                Result<CurrentUserResponse>.Failure("Unauthorized")
            );
        }

        return Task.FromResult(
            Result<CurrentUserResponse>.Success(
                new CurrentUserResponse(
                    _currentUserService.UserId.Value,
                    _currentUserService.Email ?? string.Empty,
                    _currentUserService.UserName ?? string.Empty
                    ))
        );
    }
}