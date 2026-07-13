using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery: IRequest<Result<CurrentUserResponse>>;