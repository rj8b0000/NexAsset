using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Assets.Commands.DeleteAsset;

public sealed record DeleteAssetCommand(Guid Id) : IRequest<Result>;
