using MediatR;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectDocuments.Commands.DeleteDocument;

public sealed record DeleteDocumentCommand(Guid Id) : IRequest<Result>;
