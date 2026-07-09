using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Authentication.Commands.Logout;

public sealed record LogoutCommand
    : IRequest<Result>;